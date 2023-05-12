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

	// �ʿ� ���� ����
	Vector3 forward;		// �ü� ����
	Vector3 originPos;		// ���� �� ��ġ
	Vector3 targetPos;      // ��ǥ ��ġ
	float targetMagnitude;	// originPos���� targetPos�� ���ϴ� ������ ũ��^2
	float rayLength;        // ray�� ����
	Ray ray;				// ����� ray
	RaycastHit hit;			// �浹 ������ ������ RaycastHit

	// others
	private Coroutine rushCoroutine;

	// coroutine 
	public bool isRush;

	// Constants
	public readonly int Meter = 100; // centimeter ����
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

		// ���� �� ��ġ���� ���� ��ġ�� ���ϴ� ������ ũ�Ⱑ targetMagnitude���� �۰�
		if (((unit.transform.position - originPos).magnitude < targetMagnitude))
		{
			// ������ Ray
			FDebug.DrawRay(unit.transform.position, forward * rayLength, UnityEngine.Color.red);

			// Collision�������� ������ �κ��� �޲� Ray����
			// ray�� ���̴� ���� ���ǰ� �ʿ�������...?
			if (Physics.Raycast(ray, out hit, rayLength, layer))
			{
				// ��(��ֹ�)�� �浹������ �ٷ� ���� ����
				unit.ChangeState(PlayerController.PlayerState.AttackDelay);
			}

			// while���� ���� ���� �ӵ��� moveSpeed�� ����
			unit.rigid.velocity = forward * moveSpeed;
		}
		else // targetPos�� ������ ���
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

		// �ܰ谡 �ٲ���ٸ�
		if(currentLevel != level)
		{
			currentLevel = level;
		}

		// ��ư�� Release �ƴٸ�
		if (unit.specialIsReleased)
		{
			FDebug.Log($"Level : {level}");
			unit.specialIsReleased = false;
			isRush = true;

			float attackST = unit.curNode.attackST + level * AttackSTIncreasing;
			float attackLengthMark = unit.curNode.attackLengthMark + level * LengthMarkIncreasing;

			if (rushCoroutine == null)
			{
				// �ʴ� �̵� �ӵ� ���(m/sec)
				moveSpeed = (attackLengthMark) / (Meter * unit.curNode.attackDelay);

				// �ʿ� ���� ����
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
