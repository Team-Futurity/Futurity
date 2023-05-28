using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

[FSMState((int)PlayerController.PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	private readonly float LengthMarkIncreasing = 200;	// 단계당 돌진 거리 증가량
	private readonly float AttackSTIncreasing = 1;		// 단계당 공격 배율 증가량
	private readonly float LevelStandard = 1;			// 단계를 나눌 기준
	private readonly int MaxLevel = 4;					// 최대 차지 단계
	private readonly int Meter = 100;                   // centimeter 단위
	private readonly float RangeEffectUnitLength = 0.145f; // Range 이펙트의 1unit에 해당하는 Z축 크기
	private readonly float FlyPower = 45;				// 공중 체공 힘

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
	private Collider firstEnemyCollider;       // 첫번째로 충돌한 적
	private float enemyDistance;        // 첫번째로 충돌한 적과의 거리
	private Vector3 groundPos;          // Enemy의 체공 이전 좌표
	private float originScale;		// Player의 원래 콜라이더 크기
	

	// Trigger
	public bool isReleased; // 돌진 버튼이 Release되면 true
	private bool isEnd;     // 돌진 프로세스(이동)가 종료되었는가
	private bool isLanding;	// Enemy가 착지중이라면

	// Layer
	public LayerMask wallLayer = 1 << 6; // wall Layer

	public PlayerAttackState_Charged() : base("ChargeTrigger", "Combo") { }

	// effects
		// keys
		private RushEffectData chargeEffectKey;

		// effects
		private GameObject normalAttackEffect;
		private GameObject rangeEffect;
		private GameObject rushBodyEffect;
		private GameObject rushGroundEffect;

		// etc
		private Vector3 maxRangeEffectScale;


	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		unit.attackCollider.radiusCollider.enabled = false;
		playerOriginalSpeed = unit.playerData.status.GetStatus(StatusType.SPEED).GetValue();
		attackLengthMark = unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing; // 0 Level Length Mark
		unit.playerData.status.GetStatus(StatusType.SPEED).SetValue(playerOriginalSpeed * 0.5f);
		currentTime = 0;
		currentLevel = 0;
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);

		unit.rigid.velocity = Vector3.zero;

		if (firstEnemy != null)
		{
			firstEnemy.velocity = Vector3.zero;
			firstEnemy.transform.eulerAngles = new Vector3(0, firstEnemy.rotation.eulerAngles.y, 0);
			firstEnemy.constraints = RigidbodyConstraints.FreezeAll;
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
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);

		// 디버깅용 Ray
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * ((unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing) / (Meter * unit.curNode.attackDelay) * Time.fixedDeltaTime + unit.basicCollider.radius), UnityEngine.Color.red);
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * rayLength, UnityEngine.Color.blue);

		if (!isReleased) { return; }
		if (isEnd)
		{
			firstEnemy.AddForce(Physics.gravity, ForceMode.Force); // 중력 적용
			if (firstEnemy.velocity.magnitude < 0.1f && !isLanding)
			{
				firstEnemy.velocity = Vector3.zero;
				firstEnemy.AddForce(Vector3.down * FlyPower, ForceMode.VelocityChange);
				isLanding = true;
			}

			if (firstEnemy.velocity.magnitude < 0.3f && isLanding)
			{
				unit.rushEffectManager.ActiveEffect(EffectType.AfterDoingAttack, EffectTarget.Ground, null, groundPos, null, 0, currentLevel - 1);
				firstEnemy.transform.position = groundPos;
				var unitData = firstEnemy.transform.GetComponent<UnitBase>();
				unit.playerData.Attack(unitData);
				unit.ChangeState(PlayerController.PlayerState.AttackAfterDelay);
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
				unit.rushEffectManager.ActiveEffect(EffectType.AfterDoingAttack, EffectTarget.Target, null, hit.point, Quaternion.LookRotation(hit.normal), 1);
				unit.rushEffectManager.ActiveEffect(EffectType.AfterDoingAttack, EffectTarget.Caster, null, unit.rushEffects[0].effectPos.position, Quaternion.LookRotation(hit.normal));
				
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

			if (firstEnemy != null)
			{
				firstEnemy.transform.position = targetPos + forward * (enemyDistance + moveSpeed * Time.fixedDeltaTime);
				groundPos = firstEnemy.transform.position;
				firstEnemy.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
				firstEnemyCollider.enabled = true;
				firstEnemy.AddForce(Vector3.up * FlyPower, ForceMode.VelocityChange);
				unit.rushEffectManager.ActiveEffect(EffectType.AfterDoingAttack, EffectTarget.Target, null, firstEnemy.transform.position);

				isEnd = true;

				unit.basicCollider.radius = originScale;
				unit.rigid.velocity = Vector3.zero;

				return;
			}

			unit.ChangeState(PlayerController.PlayerState.AttackAfterDelay);
		}
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);

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
				enemyDistance = unit.basicCollider.radius + halfSize * 0.5f;
				enemyRayLength = (enemyDistance) * 2 - unit.basicCollider.radius;


				// 콜라이더 조정
				unit.basicCollider.radius = originScale + 2 * halfSize;
			}
			else if(collision.body != firstEnemy) // 적과 충돌했고, 그게 처음 부딪친 적이 아니라면
			{
				var unitData = collision.transform.GetComponent<UnitBase>();
				unit.playerData.Attack(unitData);
				unitData.Knockback((collision.transform.position - unit.transform.position).normalized, LengthMarkIncreasing * 2);
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
				rangeEffect.transform.localScale = Vector3.Lerp(rangeEffect.transform.localScale, maxRangeEffectScale, LevelStandard / Time.deltaTime);
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
						chargeEffectKey = unit.rushEffectManager.ActiveLevelEffect(EffectType.Ready, EffectTarget.Caster, null, unit.rushEffects[0].effectPos.position);
						rangeEffect = unit.rushEffectManager.ActiveEffect(EffectType.Ready, EffectTarget.Ground, null, unit.transform.position, unit.transform.rotation);
						maxRangeEffectScale = new Vector3(rangeEffect.transform.localScale.x, rangeEffect.transform.localScale.y, RangeEffectUnitLength * (unit.curNode.attackLengthMark + (MaxLevel - 1) * LengthMarkIncreasing) / Meter);
					}
					else
					{
						unit.rushEffectManager.SetEffectLevel(chargeEffectKey, currentLevel - 1);
					}

					rangeEffect.transform.localScale = new Vector3(rangeEffect.transform.localScale.x, rangeEffect.transform.localScale.y, RangeEffectUnitLength * attackLengthMark/Meter);
					/*if (curEffect != null)
					{
						unit.rushObjectPool.DeactiveObject(curEffect);
					}

					unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[level - 1].effect);
					curEffect = unit.rushObjectPool.ActiveObject(unit.rushEffects[level - 1].effectPos.position);*/
				}

			}
		}
		else
		{
			FDebug.Log($"Rush Level : {currentLevel}");
			unit.specialIsReleased = false;
			isReleased = true;

			CalculateRushData(unit);

			// 돌진이라면
			if(currentLevel > 0)
			{
				// 돌진 이펙트는 1단계 이상에서만 실행

				// Remove Charge Effect
				unit.rushEffectManager.RemoveEffectByKey(chargeEffectKey);

				// Active Move Effects
				rushBodyEffect = unit.rushEffectManager.ActiveEffect(EffectType.Move, EffectTarget.Caster, unit.transform);
				rushGroundEffect = unit.rushEffectManager.ActiveEffect(EffectType.Move, EffectTarget.Ground, unit.transform);

				// 별도 처리
				originScale = unit.basicCollider.radius; 
			}
			else
			{
				normalAttackEffect = unit.rushEffectManager.ActiveEffect(EffectType.InstanceAttack, EffectTarget.Caster, unit.rushEffects[1].effectPos);
			}
			

			/*if (curEffect != null)
			{
				unit.rushObjectPool.DeactiveObject(curEffect);
			}*/

			/*unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[3].effect);
			curEffect = unit.rushObjectPool.ActiveObject(unit.rushEffects[3].effectPos.position);
			curEffect.rotation = unit.transform.rotation;
			unit.rushObjectPool2 = new ObjectPoolManager<Transform>(unit.rushEffects[4].effect);
			curEffect2 = unit.rushObjectPool2.ActiveObject(unit.rushEffects[4].effectPos.position);
			curEffect2.rotation = unit.transform.rotation;*/
		}

		currentTime += Time.deltaTime;
	}

	private void CollisionToWallProc(PlayerController unit)
	{
		if (firstEnemy != null)
		{
			// 공식 등이 작업 당시 적용되지 않아, 추가 피해는 임시로 Attack을 두 번 호출 하는 것으로 대체
			unit.playerData.Attack(firstEnemy.transform.GetComponent<UnitBase>());
			unit.playerData.Attack(firstEnemy.transform.GetComponent<UnitBase>());
		}

		// 벽(장애물)과 충돌했으니 바로 돌진 종료
		unit.ChangeState(PlayerController.PlayerState.AttackAfterDelay);
	}

	private void CalculateRushData(PlayerController unit)
	{
		// 공격 관련
		attackST = unit.curNode.attackST + currentLevel * AttackSTIncreasing;

		// 초당 이동 속도 계산(m/sec)
		moveSpeed = (attackLengthMark) / (Meter * unit.curNode.attackDelay);

		// 필요 변수 세팅
		forward = unit.transform.forward;
		originPos = unit.transform.position;
		targetPos = originPos + forward * (attackLengthMark / Meter);
		targetMagnitude = (targetPos - originPos).magnitude;
		basicRayLength = moveSpeed * Time.fixedDeltaTime + unit.basicCollider.radius;
	}

	private void RemoveEffect(PlayerController unit, GameObject effect)
	{
		if(effect != null)
		{
			unit.rushEffectManager.RemoveEffect(effect);
		}
	}

	private void RemoveEffects(PlayerController unit)
	{
		RemoveEffect(unit, rangeEffect);
		RemoveEffect(unit, rushBodyEffect);
		RemoveEffect(unit, rushGroundEffect);

		if(currentLevel > 0)
		{
			unit.rushEffectManager.RemoveEffectByKey(chargeEffectKey);
		}
	}
}
