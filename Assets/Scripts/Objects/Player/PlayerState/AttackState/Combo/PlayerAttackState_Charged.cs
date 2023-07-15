using UnityEngine;

[FSMState((int)PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	public static float LengthMarkIncreasing = 200; // 단계당 돌진 거리 증가량
	public static float AttackSTIncreasing = 1;     // 단계당 공격 배율 증가량
	public static float LevelStandard = 1;         // 단계를 나눌 기준
	public static float initialLevelStandard = 0.5f;
	public static int MaxLevel = 4;                 // 최대 차지 단계
	public static float RangeEffectUnitLength = 0.145f; // Range 이펙트의 1unit에 해당하는 Z축 크기
	public static float FlyPower = 45;               // 공중 체공 힘
	public static float WallCollisionDamage = 50f;	// 벽 충돌 시 데미지
	private readonly string KReleaseAnimKey = "KIsReleased";
	private readonly string DashEndAnimKey = "KDashEnded";
	private readonly float Sqrt2;

	// Variables
	private float playerOriginalSpeed;	// 원래 속도(속도 감쇄용)
	private int currentLevel;			// 현재 차지 단계
	private float moveSpeed;            // 돌진할 속도
	private float attackLengthMark;     // 최종적으로 이동할 거리
	private float attackST;				// 최종 공격배율
	private float targetMagnitude;		// originPos에서 targetPos로 향하는 벡터의 크기^2
	private float basicRayLength;		// ray의 기본 길이
	private float enemyRayLength;		// 적을 고려한 Ray 길이
	private float rayLength             // 최종 ray의 길이(basic + enemy)
		{ get { return basicRayLength + enemyRayLength; } }
	private RaycastHit hit;				// 충돌 정보를 가져올 RaycastHit
	private Vector3 forward;			// 시선 벡터
	private Vector3 originPos;          // 돌진 전 위치
	private Vector3 targetPos;          // 목표 위치
	private Rigidbody firstEnemy;       // 첫번째로 충돌한 적
	private Collider firstEnemyCollider;		// 첫번째로 충돌한 적의 콜라이더
	private UnitBase firstEnemyData;			// 첫번째로 충돌한 적의 데이터
	private float enemyDistance;				// 첫번째로 충돌한 적과의 거리
	private Vector3 groundPos;          // Enemy의 체공 이전 좌표
	private float originScale;		// Player의 원래 콜라이더 크기

	// Trigger
	public bool isReleased; // 돌진 버튼이 Release되면 true
	private bool isEnd;     // 돌진 프로세스(이동)가 종료되었는가
	private bool isLanding;	// Enemy가 착지중이라면

	// Layer
	public LayerMask wallLayer = 1 << 6; // wall Layer

	// Unit
	private PlayerController pc;

	public PlayerAttackState_Charged() : base("ChargeTrigger", "Combo") { Sqrt2 = Mathf.Sqrt(2); }

	// effects
		// keys
		private EffectKey chargeEffectKey;

		// effects
		private EffectKey rangeEffect;
		private EffectKey rushBodyEffect;
		private EffectKey rushGroundEffect;

		// etc
		private Vector3 maxRangeEffectScale;


	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		unit.attackCollider.radiusCollider.enabled = false;
		
		playerOriginalSpeed = unit.playerData.status.GetStatus(StatusType.SPEED).GetValue();
		attackLengthMark = unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing; // 0 Level Length Mark
		unit.playerData.status.GetStatus(StatusType.SPEED).SetValue(playerOriginalSpeed * 0.25f);
		currentTime = 0;
		currentLevel = 0;

		unit.rushGlove.SetActive(true);
		pc = unit;
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);

		unit.rigid.velocity = Vector3.zero;

		if (firstEnemy != null)
		{
			firstEnemy.velocity = Vector3.zero;
			firstEnemy.transform.eulerAngles = new Vector3(0, firstEnemy.rotation.eulerAngles.y, 0);

			// Enemy 행동 제약 해제(XZ)
			firstEnemy.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
			firstEnemyCollider.enabled = true;

			firstEnemy = null;
			firstEnemyCollider = null;
		}

		isReleased = false;
		unit.specialIsReleased = false;
		isEnd = false;
		isLanding = false;
		unit.playerData.status.GetStatus(StatusType.SPEED).SetValue(playerOriginalSpeed);

		unit.rushGlove.SetActive(false);

		RemoveEffects(unit);

		/*if(curEffect != null)
		{
			unit.rushObjectPool.DeactiveObject(curEffect);
		}*/

		pc.basicCollider.radius = originScale;
		pc.rigid.velocity = Vector3.zero;
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);

		// 디버깅용 Ray
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * ((unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing) / (PlayerController.cm2m * unit.curNode.attackDelay) * Time.fixedDeltaTime + unit.basicCollider.radius), UnityEngine.Color.red);
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * rayLength, UnityEngine.Color.blue);

		if (!isReleased) { return; }
		if (isEnd) // 돌진이 끝났고, FirstEnemy가 있었으면
		{
			// 중력 적용
			firstEnemy.AddForce(Physics.gravity, ForceMode.Force); 

			// 공중에서 정지 했는지 판별 
			if (firstEnemy.velocity.magnitude < 0.1f && !isLanding)
			{
				//DownAttack();
			}

			// 거의 다 추락했으면
			if (firstEnemy.velocity.magnitude < 0.3f && isLanding)
			{
				EnemyLanding();
			}
			return;
		}

		// 돌진 전 위치에서 현재 위치로 향하는 벡터의 크기가 targetMagnitude보다 작고
		if ((unit.transform.position - originPos).magnitude < targetMagnitude)
		{
			// 벽 연산 처리(추후 가능하면 OnCollisionEnter에서의 Wall 체크부분과 합칠 것)
			unit.SetCollider(false);

			// Collision연산으로 부족한 부분을 메꿀 Ray연산
			// ray의 길이는 조금 논의가 필요할지도...?
			if (Physics.Raycast(unit.transform.position, forward, out hit, rayLength, wallLayer))
			{
				/*unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[5].effect);
				curEffect = unit.rushObjectPool.ActiveObject(point.point);*/
				unit.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Target, hit.point, Quaternion.LookRotation(hit.normal), null, 1);
				unit.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Caster, unit.rushEffects[0].effectPos.position, Quaternion.LookRotation(hit.normal), unit.gameObject);
				
				CollisionToWallProc(unit);
			}

			unit.SetCollider(true);

			// 이동
			unit.rigid.velocity = forward * moveSpeed;

			if (firstEnemy != null)
			{
				firstEnemy.transform.position = unit.transform.position + forward * enemyDistance;
			}
		}
		else // targetPos에 도달한 경우
		{
			// 위치 보정
			unit.transform.position = targetPos;

			if (currentLevel > 0)
			{
				unit.animator.SetTrigger(DashEndAnimKey);
			}

			if (firstEnemy != null)
			{
				// Enemy 위치 보정
				firstEnemy.transform.position = targetPos + forward * (enemyDistance + moveSpeed * Time.fixedDeltaTime);

				pc = unit;

				unit.animator.SetTrigger("KDashLastAttack");

				//UpAttack();

				return;
			}

			unit.ChangeState(PlayerState.AttackAfterDelay);
		}
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);

		if(!isReleased) { return; }

		if (collision.transform.CompareTag(unit.EnemyTag))
		{
			// 돌진 중 한 번도 적과 충돌한 적 없다면
			if (firstEnemy == null)
			{
				// enemy의 콜라이더 절반 크기 계산
				Vector3 localScale = collision.transform.localScale;
				float halfSize = Vector3.Dot(localScale, (collision.transform.position - unit.transform.position).normalized);

				// Enemy 기본 세팅
				firstEnemy = collision.rigidbody;
				firstEnemy.constraints = RigidbodyConstraints.FreezeRotation;
				firstEnemyCollider = collision.collider;
				firstEnemyCollider.enabled = false;
				firstEnemyData = collision.transform.GetComponent<UnitBase>();
				enemyDistance = unit.basicCollider.radius + halfSize * 0.5f;
				enemyRayLength = (enemyDistance) * 2 - unit.basicCollider.radius;

				// 콜라이더 조정
				unit.basicCollider.radius = originScale + 2 * halfSize;

				// 충돌 데미지 처리
				unit.playerData.Attack(firstEnemyData, attackNode.attackST);
			}
			else if(collision.body != firstEnemy) // 적과 충돌했고, 그게 처음 부딪친 적이 아니라면
			{
				var unitData = collision.transform.GetComponent<UnitBase>();
				unit.playerData.Attack(unitData, attackNode.attackST);

				Vector3 targetDir = (collision.transform.position - unit.transform.position).normalized;
				Vector3 knockbackDir = Vector3.Dot(Vector3.Cross(unit.transform.forward, targetDir), Vector3.up) > 0 ? unit.transform.right : -unit.transform.right;
				unitData.Knockback(knockbackDir, LengthMarkIncreasing * 2);
			}
			return;
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			CollisionToWallProc(unit);
			return;
		}
	}

	public override void Update(PlayerController unit)
	{
		int level = 0;

		if (isReleased) { return; }

		if (!unit.specialIsReleased)
		{
			level = (int)(currentTime / LevelStandard);
			level = Mathf.Clamp(level, 0, MaxLevel - 1);

			if(rangeEffect != null)
			{
				rangeEffect.EffectObject.transform.localScale = Vector3.Lerp(rangeEffect.EffectObject.transform.localScale, maxRangeEffectScale, LevelStandard / Time.deltaTime);
			}

			// 단계가 바뀌었다면
			if (currentLevel != level)
			{
				currentLevel = level;
				attackLengthMark = unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing;

				if (currentLevel > 0)
				{
					if(currentLevel == 1)
					{
						chargeEffectKey = unit.effectManager.ActiveEffect(EffectActivationTime.AttackReady, EffectTarget.Caster, unit.rushEffects[0].effectPos.position, null, unit.gameObject);
						unit.effectManager.RegistLevelEffect(chargeEffectKey);
						rangeEffect = unit.effectManager.ActiveEffect(EffectActivationTime.AttackReady, EffectTarget.Ground, unit.transform.position, unit.transform.rotation);
						maxRangeEffectScale = new Vector3(rangeEffect.EffectObject.transform.localScale.x, rangeEffect.EffectObject.transform.localScale.y, RangeEffectUnitLength * (unit.curNode.attackLengthMark + (MaxLevel - 1) * LengthMarkIncreasing) * PlayerController.cm2m);
					}
					else
					{
						unit.effectManager.SetEffectLevel(ref chargeEffectKey, currentLevel - 1);
					}

					unit.animator.SetInteger(unit.currentAttackAnimKey, currentLevel);
					rangeEffect.EffectObject.transform.localScale = new Vector3(rangeEffect.EffectObject.transform.localScale.x, rangeEffect.EffectObject.transform.localScale.y, RangeEffectUnitLength * attackLengthMark * PlayerController.cm2m);
				}

			}
		}
		else
		{
			FDebug.Log($"Rush Level : {currentLevel}");
			unit.specialIsReleased = false;
			isReleased = true;
			unit.animator.SetTrigger(KReleaseAnimKey);

			CalculateRushData(unit);

			// 돌진이라면
			if(currentLevel > 0)
			{
				// 돌진 이펙트는 1단계 이상에서만 실행

				// Remove Charge Effect
				unit.effectManager.RemoveEffect(chargeEffectKey, null, true);
				unit.effectManager.RemoveEffect(rangeEffect);

				// Active Move Effects
				rushBodyEffect = unit.effectManager.ActiveEffect(EffectActivationTime.MoveWhileAttack, EffectTarget.Caster);
				unit.effectManager.RegisterTracking(rushBodyEffect, unit.rushEffects[2].effectPos);
				rushGroundEffect = unit.effectManager.ActiveEffect(EffectActivationTime.MoveWhileAttack, EffectTarget.Ground);
				unit.effectManager.RegisterTracking(rushGroundEffect, unit.transform);

				// 별도 처리
				originScale = unit.basicCollider.radius; 
			}
			else
			{
				unit.playerAnimationEvents.AllocateEffect(EffectActivationTime.InstanceAttack, EffectTarget.Caster, unit.rushEffects[1].effectPos);
			}
		}

		currentTime += Time.deltaTime;
	}

	private void CollisionToWallProc(PlayerController unit)
	{
		if (firstEnemy != null)
		{
			firstEnemyData.Hit(unit.playerData, WallCollisionDamage);
		}

		if (currentLevel > 0)
		{
			unit.animator.SetTrigger(DashEndAnimKey);
		}

		// 벽(장애물)과 충돌했으니 바로 돌진 종료
		unit.ChangeState(PlayerState.AttackAfterDelay);
	}

	private void CalculateRushData(PlayerController unit)
	{
		// 공격 관련
		attackST = unit.curNode.attackST + currentLevel * AttackSTIncreasing;

		// 초당 이동 속도 계산(m/sec)
		moveSpeed = (attackLengthMark * PlayerController.cm2m) / (unit.curNode.attackDelay);

		// 필요 변수 세팅
		forward = unit.transform.forward;
		originPos = unit.transform.position;
		targetPos = originPos + forward * (attackLengthMark * PlayerController.cm2m);
		targetMagnitude = (targetPos - originPos).magnitude;
		basicRayLength = moveSpeed * Time.fixedDeltaTime + Sqrt2 * unit.basicCollider.radius;
	}

	private void RemoveEffect(PlayerController unit, EffectKey effect)
	{
		// key가 있고 오브젝트가 활성화되어 있다면
		if(effect != null && effect.EffectObject.activeSelf)
		{
			unit.effectManager.RemoveEffect(effect);
		}
	}

	private void RemoveEffects(PlayerController unit)
	{
		RemoveEffect(unit, rangeEffect);
		RemoveEffect(unit, rushBodyEffect);
		RemoveEffect(unit, rushGroundEffect);

		if(currentLevel > 0)
		{
			RemoveEffect(unit, chargeEffectKey);
		}
	}

	public void UpAttack()
	{
		// Enemy 초기 위치를 Ground로 지정
		groundPos = firstEnemy.transform.position;

		// Enemy 행동 제약 해제(XZ)
		firstEnemy.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
		firstEnemyCollider.enabled = true;

		// Enemy 날리기 
		firstEnemy.AddForce(Vector3.up * FlyPower, ForceMode.VelocityChange);
		pc.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Target, firstEnemy.transform.position);

		// 돌진이 끝났음을 알림
		isEnd = true;

		pc.basicCollider.radius = originScale;
		pc.rigid.velocity = Vector3.zero;
	}

	public void DownAttack()
	{
		// 속도 초기화 후 아래로 힘을 가함
		firstEnemy.velocity = Vector3.zero;
		firstEnemy.AddForce(Vector3.down * FlyPower, ForceMode.VelocityChange);

		// Landing 상태가 됨을 알림
		isLanding = true;
	}

	public void EnemyLanding()
	{
		// 이팩트 출력
		pc.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Ground, groundPos, null, null, 0, currentLevel - 1);

		// 포지션 정상화
		firstEnemy.transform.position = groundPos;

		// 데미지 처리
		pc.playerData.Attack(firstEnemyData, attackST);

		// State Change
		pc.ChangeState(PlayerState.AttackAfterDelay);
	}
}
