using UnityEngine;

[FSMState((int)PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	public static ChargeIncreases[] IncreasesByLevel;
	public static int MaxLevel = 4;                 // 최대 차지 단계
	public static float RangeEffectUnitLength = 0.145f; // Range 이펙트의 1unit에 해당하는 Z축 크기
	public static float FlyPower = 45;               // 공중 체공 힘
	public static float WallCollisionDamage = 50f;  // 벽 충돌 시 데미지
	public static float MoveSpeedInCharging = 0.5f;
	public static ChargeCollisionData ChargeCollisionData;
	public static GameObject ChargeCollisionEffect;
	private readonly string KReleaseAnimKey = "KIsReleased";
	private readonly string DashEndAnimKey = "KDashEnded";
	private readonly float Sqrt2;

	// Variables
	private float playerOriginalSpeed;	// 원래 속도(속도 감쇄용)
	private int currentLevel;			// 현재 차지 단계
	private float moveSpeed;            // 돌진할 속도
	private float attackLengthMark;     // 최종적으로 이동할 거리
	private float attackKnockback;     // 최종적으로 이동할 거리
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

	// effects
		// keys
		private EffectKey chargeEffectKey;

		// effects
		private EffectKey rangeEffect;
		private EffectKey rushBodyEffect;
		private EffectKey rushGroundEffect;
		private ObjectPoolManager<Transform> collisionToWallEffectPoolManager;

		// etc
		private Vector3 maxRangeEffectScale;
		private GameObject chargeEffectPos;

	public PlayerAttackState_Charged(StateData stateData) : base(stateData, "ChargeTrigger", "Combo") 
	{ 
		Sqrt2 = Mathf.Sqrt(2);
		stateData.SetDataToState();
		chargeEffectPos = new GameObject("Charge Effect Position");
		collisionToWallEffectPoolManager = new ObjectPoolManager<Transform>(ChargeCollisionEffect);
	}

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		unit.attackColliderChanger.DisableAllCollider();
		
		playerOriginalSpeed = unit.playerData.status.GetStatus(StatusType.SPEED).GetValue();
		attackLengthMark = unit.curNode.attackLengthMark + IncreasesByLevel[currentLevel].LengthMarkIncreasing; // 0 Level Length Mark
		unit.playerData.status.GetStatus(StatusType.SPEED).SetValue(playerOriginalSpeed * 0.5f);
		currentTime = 0;
		currentLevel = 0;

		// effect
		chargeEffectPos.transform.parent = unit.transform;
		chargeEffectKey = unit.effectController.ActiveEffect(EffectActivationTime.AttackReady, EffectTarget.Caster, null, null, unit.playerEffectParent);
		unit.effectController.RegistLevelEffect(chargeEffectKey);
		unit.effectController.RegisterTracking(chargeEffectKey, chargeEffectPos.transform);
		rangeEffect = unit.effectController.ActiveEffect(EffectActivationTime.AttackReady, EffectTarget.Ground, unit.transform.position + Vector3.up * 0.01f, unit.transform.rotation, unit.playerEffectParent);
		maxRangeEffectScale = new Vector3(rangeEffect.EffectObject.transform.localScale.x, rangeEffect.EffectObject.transform.localScale.y, RangeEffectUnitLength * (unit.curNode.attackLengthMark + IncreasesByLevel[MaxLevel - 1].LengthMarkIncreasing) * MathPlus.cm2m);

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
		FDebug.DrawLine(unit.transform.position, targetPos, Color.red);

		if (!isReleased) { SetPostionChargeEffect(unit);  return; }

		// 돌진 전 위치에서 현재 위치로 향하는 벡터의 크기가 targetMagnitude보다 작고
		if ((unit.transform.position - originPos).magnitude < targetMagnitude)
		{
			// 이동
			unit.rigid.velocity = forward * moveSpeed;
		}
		else // targetPos에 도달한 경우
		{
			// 위치 보정
			unit.transform.position = targetPos;

			unit.animator.SetTrigger(DashEndAnimKey);

			NextAttackState(unit, PlayerState.AttackAfterDelay);
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

		bool isEnemy = collision.transform.CompareTag(unit.EnemyTag);
		if (isEnemy || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			UnitBase unitData = null;
			if(isEnemy)
			{
				Vector3 knockbackDir = unit.transform.forward;

				var gotoWall = collision.gameObject.GetComponent<ActiveEffectToWall>();
				if(gotoWall == null)
				{
					gotoWall = collision.gameObject.AddComponent<ActiveEffectToWall>();
				}
				gotoWall.RunCollision(ChargeCollisionData, collisionToWallEffectPoolManager, collision.rigidbody, unit.camera);


				unitData = collision.transform.GetComponent<UnitBase>();
				unitData.Knockback(knockbackDir, attackKnockback * 2);

				DamageInfo info = new DamageInfo(unit.playerData, unitData, attackNode.attackST);
				unit.playerData.Attack(info);
			}

			CollisionProcess(unit, unitData);
		}
	}

	public override void Update(PlayerController unit)
	{
		int level = 0;

		if (isReleased) { return; }

		if (!unit.specialIsReleased)
		{
			level = currentTime > IncreasesByLevel[currentLevel].LevelStandard ? currentLevel + 1 : currentLevel;
			//level = (int)(currentTime / LevelStandard);
			level = Mathf.Clamp(level, 0, MaxLevel - 1);

			FDebug.Log(rangeEffect);
			if (rangeEffect != null)
			{
				float spendingTime = currentLevel == 0 ? IncreasesByLevel[currentLevel].LevelStandard : IncreasesByLevel[currentLevel].LevelStandard - IncreasesByLevel[currentLevel - 1].LevelStandard;
				rangeEffect.EffectObject.transform.localScale = Vector3.Lerp(rangeEffect.EffectObject.transform.localScale, maxRangeEffectScale, spendingTime * Time.deltaTime);
			}

			// 단계가 바뀌었다면
			if (currentLevel != level)
			{
				currentLevel = level;

				unit.effectController.SetEffectLevel(ref chargeEffectKey, currentLevel);

				FDebug.Log(currentLevel + "_" + chargeEffectKey.EffectObject.name + "_" + chargeEffectKey.IsTrackingEffect());

				unit.animator.SetInteger(unit.currentAttackAnimKey, currentLevel);
				rangeEffect.EffectObject.transform.localScale = new Vector3(rangeEffect.EffectObject.transform.localScale.x, rangeEffect.EffectObject.transform.localScale.y, RangeEffectUnitLength * attackLengthMark * MathPlus.cm2m);
			}
		}
		else
		{
			FDebug.Log($"Rush Level : {currentLevel}");
			unit.specialIsReleased = false;
			isReleased = true;
			unit.animator.SetTrigger(KReleaseAnimKey);

			CalculateRushData(unit);

			// Remove Charge Effect
			unit.effectController.RemoveEffect(chargeEffectKey, null, true);
			unit.effectController.RemoveEffect(rangeEffect);

			// Active Move Effects
			rushBodyEffect = unit.effectController.ActiveEffect(EffectActivationTime.MoveWhileAttack, EffectTarget.Caster, null, null, unit.playerEffectParent);
			unit.effectController.RegisterTracking(rushBodyEffect, unit.rushEffects[2].effectPos);
			rushGroundEffect = unit.effectController.ActiveEffect(EffectActivationTime.MoveWhileAttack, EffectTarget.Ground, null, null, unit.playerEffectParent);
			unit.effectController.RegisterTracking(rushGroundEffect, unit.transform);

			// 별도 처리
			originScale = unit.basicCollider.radius;
		}

		currentTime += Time.deltaTime;
	}

	private void CollisionProcess(PlayerController unit, UnitBase target = null)
	{
		if (target != null)
		{
			DamageInfo info = new DamageInfo(unit.playerData, target, attackST);
			unit.playerData.Attack(info);
		}

		if (currentLevel > 0)
		{
			unit.animator.SetTrigger(DashEndAnimKey);
		}

		unit.rigid.velocity = Vector3.zero;

		// 벽(장애물)과 충돌했으니 바로 돌진 종료
		NextAttackState(unit, PlayerState.AttackAfterDelay);
		//unit.ChangeState(PlayerState.AttackAfterDelay);
	}

	private void CalculateRushData(PlayerController unit)
	{
		// 레벨 별 데이터
		attackST = unit.curNode.attackST + IncreasesByLevel[currentLevel].AttackSTIncreasing;
		attackLengthMark = unit.curNode.attackLengthMark + IncreasesByLevel[currentLevel].LengthMarkIncreasing;
		attackKnockback = unit.curNode.attackKnockback + IncreasesByLevel[currentLevel].KnockbackIncreasing;

		// 초당 이동 속도 계산(m/sec)
		moveSpeed = (attackLengthMark * MathPlus.cm2m) / (unit.curNode.attackDelay);

		// 필요 변수 세팅
		forward = unit.transform.forward;
		originPos = unit.transform.position;
		targetPos = originPos + forward * (attackLengthMark * MathPlus.cm2m);
		targetMagnitude = (targetPos - originPos).magnitude;
		basicRayLength = moveSpeed * Time.fixedDeltaTime + Sqrt2 * unit.basicCollider.radius;
	}

	private void RemoveEffect(PlayerController unit, EffectKey effect)
	{
		// key가 있고 오브젝트가 활성화되어 있다면
		if(effect != null && effect.EffectObject.activeSelf)
		{
			unit.effectController.RemoveEffect(effect);
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

	/*public void UpAttack()
	{
		// Enemy 초기 위치를 Ground로 지정
		groundPos = firstEnemy.transform.position;

		// Enemy 행동 제약 해제(XZ)
		firstEnemy.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
		firstEnemyCollider.enabled = true;

		// Enemy 날리기 
		firstEnemy.AddForce(Vector3.up * FlyPower, ForceMode.VelocityChange);
		pc.effectController.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Target, firstEnemy.transform.position);

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
		pc.effectController.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Ground, groundPos, null, null, 0, currentLevel - 1);

		// 포지션 정상화
		firstEnemy.transform.position = groundPos;

		// 데미지 처리
		DamageInfo info = new DamageInfo(pc.playerData, firstEnemyData, attackST);
		pc.playerData.Attack(info);

		// State Change
		NextAttackState(pc, PlayerState.AttackAfterDelay);
		//pc.ChangeState(PlayerState.AttackAfterDelay);
	}*/

	private void SetPostionChargeEffect(PlayerController unit)
	{
		Vector3 midPosition = (unit.hands[0].transform.position + unit.hands[1].transform.position) * 0.5f;
		chargeEffectPos.transform.position = midPosition;
	}
}
