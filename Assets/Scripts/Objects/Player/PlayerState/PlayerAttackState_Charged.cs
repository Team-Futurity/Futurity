using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	private readonly float LengthMarkIncreasing = 200;
	private readonly float AttackSTIncreasing = 1;
	private readonly float LevelStandard = 1;
	private readonly int MaxLevel = 4;

	// Variables
	private float playerOriginalSpeed;
	private int currentLevel;
	private float currentTime;
	private float moveSpeed;

	// 필요 변수 세팅
	Vector3 forward;		// 시선 벡터
	Vector3 originPos;		// 돌진 전 위치
	Vector3 targetPos;      // 목표 위치
	float targetMagnitude;	// originPos에서 targetPos로 향하는 벡터의 크기^2
	float rayLength;        // ray의 길이
	Ray ray;				// 사용할 ray
	RaycastHit hit;			// 충돌 정보를 가져올 RaycastHit

	// others
	private Coroutine rushCoroutine;

	// coroutine 
	public bool isRush;

	// Constants
	public readonly int Meter = 100; // centimeter 단위
	public LayerMask layer = 1 << 6;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		playerOriginalSpeed = unit.playerData.Speed;
		unit.playerData.SetSpeed(unit.playerData.Speed * 0.5f);
		currentTime = 0;
		currentLevel = 0;
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);
		unit.rigid.velocity = Vector3.zero;
		isRush = false;
		unit.playerData.SetSpeed(playerOriginalSpeed);
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);

		if (!isRush) { return; }

		// 돌진 전 위치에서 현재 위치로 향하는 벡터의 크기가 targetMagnitude보다 작고
		if (((unit.transform.position - originPos).magnitude < targetMagnitude))
		{
			// 디버깅용 Ray
			FDebug.DrawRay(unit.transform.position, forward * rayLength, UnityEngine.Color.red);

			// Collision연산으로 부족한 부분을 메꿀 Ray연산
			// ray의 길이는 조금 논의가 필요할지도...?
			if (Physics.Raycast(ray, out hit, rayLength, layer))
			{
				// 벽(장애물)과 충돌했으니 바로 돌진 종료
				unit.ChangeState(PlayerController.PlayerState.AttackDelay);
			}

			// while문이 도는 동안 속도를 moveSpeed로 고정
			unit.rigid.velocity = forward * moveSpeed;
		}
		else // targetPos에 도달한 경우
		{
			unit.transform.position = targetPos;
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
	}

	public override void Update(PlayerController unit)
	{
		int level = (int)(currentTime / LevelStandard);
		level = Mathf.Clamp(level, 0, MaxLevel - 1);

		// 단계가 바뀌었다면
		if(currentLevel != level)
		{
			currentLevel = level;
		}

		// 버튼이 Release 됐다면
		if (unit.specialIsReleased)
		{
			FDebug.Log($"Level : {level}");
			unit.specialIsReleased = false;
			isRush = true;

			float attackST = unit.curNode.attackST + level * AttackSTIncreasing;
			float attackLengthMark = unit.curNode.attackLengthMark + level * LengthMarkIncreasing;

			if (rushCoroutine == null)
			{
				// 초당 이동 속도 계산(m/sec)
				moveSpeed = (attackLengthMark) / (Meter * unit.curNode.attackDelay);

				// 필요 변수 세팅
				forward = unit.transform.forward;
				originPos = unit.transform.position;
				targetPos = originPos + forward * (attackLengthMark / Meter);
				targetMagnitude = (targetPos - originPos).magnitude;
				rayLength = moveSpeed * Time.fixedDeltaTime + unit.basicCollider.radius;
				ray = new Ray(originPos, forward);
			}
		}

		currentTime += Time.deltaTime;
	}
}
