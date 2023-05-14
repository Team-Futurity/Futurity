using System.Collections;
using System.Collections.Generic;
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
	private float currentTime;			// 지난 시간
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
	public LayerMask layer = 1 << 6; // wall Layer

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		playerOriginalSpeed = unit.playerData.status.GetStatus(StatusName.SPEED);
		unit.playerData.status.SetStatus(StatusName.SPEED, playerOriginalSpeed * 0.5f);
		currentTime = 0;
		currentLevel = 0;

		unit.specialIsReleased = false;
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);

		unit.rigid.velocity = Vector3.zero;

		if(firstEnemy != null )
		{
			firstEnemy.velocity = Vector3.zero;
			firstEnemy.transform.eulerAngles = new Vector3(0, firstEnemy.rotation.eulerAngles.y, 0);
			firstEnemy = null;
		}

		isReleased = false;
		unit.specialIsReleased = false;
		unit.playerData.status.SetStatus(StatusName.SPEED, playerOriginalSpeed);
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);

		// 디버깅용 Ray
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * ((unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing) / (Meter * unit.curNode.attackDelay) * Time.fixedDeltaTime + unit.basicCollider.radius), UnityEngine.Color.red);
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * rayLength, UnityEngine.Color.blue);

		if (!isReleased) { return; }

		// 돌진 전 위치에서 현재 위치로 향하는 벡터의 크기가 targetMagnitude보다 작고
		if (((unit.transform.position - originPos).magnitude < targetMagnitude))
		{
			unit.SetCollider(false);

			// Collision연산으로 부족한 부분을 메꿀 Ray연산
			// ray의 길이는 조금 논의가 필요할지도...?
			if (Physics.Raycast(unit.transform.position, forward, out hit, rayLength, layer))
			{
				// 공식 등이 정상 적용되지 않아, 추가 피해는 임시로 Attack을 두 번 호출 하는 것으로 대체
				unit.playerData.Attack(hit.transform.GetComponent<UnitBase>());
				unit.playerData.Attack(hit.transform.GetComponent<UnitBase>());

				// 벽(장애물)과 충돌했으니 바로 돌진 종료
				unit.ChangeState(PlayerController.PlayerState.AttackDelay);
			}

			unit.SetCollider(true);

			// while문이 도는 동안 속도를 moveSpeed로 고정
			unit.rigid.velocity = forward * moveSpeed;
		}
		else // targetPos에 도달한 경우
		{
			FDebug.Log("End");
			unit.transform.position = targetPos;

			if(firstEnemy != null)
			{
				firstEnemy.transform.position = targetPos + forward * (enemyDistance + moveSpeed * Time.fixedDeltaTime);
			}
			unit.ChangeState(PlayerController.PlayerState.AttackDelay);
		}
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);

		if(collision.transform.CompareTag(unit.EnemyTag))
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
				// 넉백 코드
				// ...
				var unitData = collision.transform.GetComponent<UnitBase>();
				unit.playerData.Attack(unitData);
			}
		}
	}

	public override void Update(PlayerController unit)
	{
		int level = 0;

		// 버튼이 눌려있는 상태라면
		if (!unit.specialIsReleased)
		{
			level = (int)(currentTime / LevelStandard);
			level = Mathf.Clamp(level, 0, MaxLevel - 1);

			// 단계가 바뀌었다면
			if (currentLevel != level)
			{
				currentLevel = level;
			}
		}
		else
		{
			FDebug.Log($"Level : {currentLevel}");
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
		}

		if (firstEnemy != null)
		{
			firstEnemy.transform.position = unit.transform.position + forward * enemyDistance;
		}

		currentTime += Time.deltaTime;
	}
}
