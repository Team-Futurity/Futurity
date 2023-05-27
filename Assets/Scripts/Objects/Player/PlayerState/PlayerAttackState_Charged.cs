using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	private readonly float LengthMarkIncreasing = 200;	// 단계당 돌진 거리 증가량
	private readonly float AttackSTIncreasing = 1;		// 단계당 공격 배율 증가량
	private readonly float LevelStandard = 1;			// 단계를 나눌 기준
	private readonly int MaxLevel = 4;					// 최대 차지 단계
	private readonly int Meter = 100;					// centimeter 단위

	// Variables
	private float playerOriginalSpeed;	// 원래 속도(속도 감쇄용)
	private int currentLevel;			// 현재 차지 단계
	private float moveSpeed;			// 돌진할 속도
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
	private float enemyDistance;		// 첫번째로 충돌한 적과의 거리

	// Trigger
	public bool isReleased;	// 돌진 버튼이 Release되면 true

	// Layer
	public LayerMask wallLayer = 1 << 6; // wall Layer

	public PlayerAttackState_Charged() : base("ChargeTrigger", "Combo") { }

	// effects
	// keys
	private RushEffectData chargeEffectKey;

	// effects
	private GameObject rushBodyEffect;
	private GameObject rushGroundEffect;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		unit.attackCollider.radiusCollider.enabled = false;
		playerOriginalSpeed = unit.playerData.status.GetStatus(StatusType.SPEED).GetValue();
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
			firstEnemy = null;
		}

		isReleased = false;
		unit.specialIsReleased = false;
		unit.playerData.status.GetStatus(StatusType.SPEED).SetValue(playerOriginalSpeed);
		
		if(curEffect != null)
		{
			unit.rushObjectPool.DeactiveObject(curEffect);
		}
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);

		// 디버깅용 Ray
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * ((unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing) / (Meter * unit.curNode.attackDelay) * Time.fixedDeltaTime + unit.basicCollider.radius), UnityEngine.Color.red);
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * rayLength, UnityEngine.Color.blue);

		if (!isReleased) { return; }

		// 이펙트 업데이트
		if(curEffect != null)
		{
			curEffect.transform.position = unit.rushEffects[3].effectPos.position;
		}

		if (curEffect2 != null)
		{
			curEffect2.transform.position = unit.rushEffects[4].effectPos.position;
		}


		// 돌진 전 위치에서 현재 위치로 향하는 벡터의 크기가 targetMagnitude보다 작고
		if (((unit.transform.position - originPos).magnitude < targetMagnitude))
		{
			unit.rigid.velocity = forward * moveSpeed;
		}
		else // targetPos에 도달한 경우
		{
			unit.transform.position = targetPos;

			if (firstEnemy != null)
			{
				firstEnemy.transform.position = targetPos + forward * (enemyDistance + moveSpeed * Time.fixedDeltaTime);
			}

			if (curEffect != null)
			{
				unit.rushObjectPool.DeactiveObject(curEffect);
			}

			if (curEffect2 != null)
			{
				unit.rushObjectPool2.DeactiveObject(curEffect2);
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
				firstEnemy = collision.transform.GetComponent<Rigidbody>();
				firstEnemy.constraints = RigidbodyConstraints.FreezeRotation;
				enemyDistance = unit.basicCollider.radius + collision.collider.bounds.size.x * 0.5f;
				enemyRayLength = (enemyDistance) * 2 - unit.basicCollider.radius;
			}
			else // 적과 충돌했었다면
			{
				var unitData = collision.transform.GetComponent<UnitBase>();
				unit.playerData.Attack(unitData);
				unitData.Knockback(collision.GetContact(0).normal, LengthMarkIncreasing);
			}
			return;
		}
		else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
		{
			if (curEffect != null)
			{
				unit.rushObjectPool.DeactiveObject(curEffect);
			}

			if (curEffect2 != null)
			{
				unit.rushObjectPool2.DeactiveObject(curEffect2);
			}

			ContactPoint point = collision.GetContact(0);

			unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[5].effect);
			curEffect = unit.rushObjectPool.ActiveObject(point.point);

			CollisionToWallProc(unit);
			return;
		}
	}

	public override void Update(PlayerController unit)
	{
		int level = 0;

		if (firstEnemy != null)
		{
			firstEnemy.transform.position = unit.transform.position + forward * enemyDistance;
		}

		if(isReleased) { return; }

		if (!unit.specialIsReleased)
		{
			level = (int)(currentTime / LevelStandard);
			level = Mathf.Clamp(level, 0, MaxLevel - 1);

			// 단계가 바뀌었다면
			if (currentLevel != level)
			{
				currentLevel = level;
				FDebug.Log(currentLevel);

				if (currentLevel > 0)
				{
					if(currentLevel == 1)
					{
						chargeEffectKey = unit.rushEffectManager.ActiveLevelEffect(EffectType.Ready, EffectTarget.Caster, 0, null, unit.rushEffects[0].effectPos.position);
					}
					else
					{
						unit.rushEffectManager.SetEffectLevel(chargeEffectKey, currentLevel - 1);
					}
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

			float attackST = unit.curNode.attackST + currentLevel * AttackSTIncreasing;
			float attackLengthMark = unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing;

			// 초당 이동 속도 계산(m/sec)
			moveSpeed = (attackLengthMark) / (Meter * unit.curNode.attackDelay);

			// 필요 변수 세팅
			forward = unit.transform.forward;
			originPos = unit.transform.position;
			targetPos = originPos + forward * (attackLengthMark / Meter);
			targetMagnitude = (targetPos - originPos).magnitude;
			basicRayLength = moveSpeed * Time.fixedDeltaTime + unit.basicCollider.radius;

			/*if (curEffect != null)
			{
				unit.rushObjectPool.DeactiveObject(curEffect);
			}*/

			unit.rushEffectManager.RemoveEffectByKey(chargeEffectKey);

			/*unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[3].effect);
			curEffect = unit.rushObjectPool.ActiveObject(unit.rushEffects[3].effectPos.position);
			curEffect.rotation = unit.transform.rotation;
			unit.rushObjectPool2 = new ObjectPoolManager<Transform>(unit.rushEffects[4].effect);
			curEffect2 = unit.rushObjectPool2.ActiveObject(unit.rushEffects[4].effectPos.position);
			curEffect2.rotation = unit.transform.rotation;*/

			rushBodyEffect = unit.rushEffectManager.ActiveEffect(EffectType.Move, EffectTarget.Caster, 0, 0, unit.transform);
			rushGroundEffect = unit.rushEffectManager.ActiveEffect(EffectType.Move, EffectTarget.Ground, 0, 0, unit.transform);
		}

		currentTime += Time.deltaTime;
	}

	private void CollisionToWallProc(PlayerController unit)
	{
		if(firstEnemy != null) 
		{
			// 공식 등이 정상 적용되지 않아, 추가 피해는 임시로 Attack을 두 번 호출 하는 것으로 대체
			unit.playerData.Attack(firstEnemy.transform.GetComponent<UnitBase>());
			unit.playerData.Attack(firstEnemy.transform.GetComponent<UnitBase>());
		}

		// 벽(장애물)과 충돌했으니 바로 돌진 종료
		unit.ChangeState(PlayerController.PlayerState.AttackAfterDelay);
	}
}
