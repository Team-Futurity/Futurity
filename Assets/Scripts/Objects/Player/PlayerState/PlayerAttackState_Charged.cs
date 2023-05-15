using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	private readonly float LengthMarkIncreasing = 200;	// �ܰ�� ���� �Ÿ� ������
	private readonly float AttackSTIncreasing = 1;		// �ܰ�� ���� ���� ������
	private readonly float LevelStandard = 1;			// �ܰ踦 ���� ����
	private readonly int MaxLevel = 4;					// �ִ� ���� �ܰ�
	private readonly int Meter = 100;					// centimeter ����

	// Variables
	private float playerOriginalSpeed;	// ���� �ӵ�(�ӵ� �����)
	private int currentLevel;			// ���� ���� �ܰ�
	private float currentTime;			// ���� �ð�
	private float moveSpeed;			// ������ �ӵ�
	private float targetMagnitude;		// originPos���� targetPos�� ���ϴ� ������ ũ��^2
	private float basicRayLength;		// ray�� �⺻ ����
	private float enemyRayLength;		// ���� ����� Ray ����
	private float rayLength             // ���� ray�� ����(basic + enemy)
		{ get { return basicRayLength + enemyRayLength; } }
	private RaycastHit hit;				// �浹 ������ ������ RaycastHit
	private Vector3 forward;			// �ü� ����
	private Vector3 originPos;          // ���� �� ��ġ
	private Vector3 targetPos;          // ��ǥ ��ġ
	private Rigidbody firstEnemy;       // ù��°�� �浹�� ��
	private float enemyDistance;		// ù��°�� �浹�� ������ �Ÿ�

	// Trigger
	public bool isReleased;	// ���� ��ư�� Release�Ǹ� true

	// Layer
	public LayerMask wallLayer = 1 << 6; // wall Layer

	private Transform curEffect;
	private Transform curEffect2;

	public PlayerAttackState_Charged() : base("ChargeTrigger", "Combo") { }

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		playerOriginalSpeed = unit.playerData.status.GetStatus(StatusType.SPEED).GetValue();
		unit.playerData.status.GetStatus(StatusType.SPEED).SetValue(playerOriginalSpeed * 0.5f);
		currentTime = 0;
		currentLevel = 0;

		//unit.specialIsReleased = false;
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

		// ������ Ray
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * ((unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing) / (Meter * unit.curNode.attackDelay) * Time.fixedDeltaTime + unit.basicCollider.radius), UnityEngine.Color.red);
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * rayLength, UnityEngine.Color.blue);

		if (!isReleased) { return; }

		// ����Ʈ ������Ʈ
		if(curEffect != null)
		{
			curEffect.transform.position = unit.rushEffects[3].effectPos.position;
			curEffect.transform.rotation = unit.transform.rotation;
		}

		if (curEffect2 != null)
		{
			curEffect2.transform.position = unit.rushEffects[4].effectPos.position;
			curEffect2.transform.rotation = unit.transform.rotation;
		}


		// ���� �� ��ġ���� ���� ��ġ�� ���ϴ� ������ ũ�Ⱑ targetMagnitude���� �۰�
		if (((unit.transform.position - originPos).magnitude < targetMagnitude))
		{
			unit.SetCollider(false);

			// Collision�������� ������ �κ��� �޲� Ray����
			// ray�� ���̴� ���� ���ǰ� �ʿ�������...?
			if (Physics.Raycast(unit.transform.position, forward, out hit, rayLength, wallLayer))
			{
				//CollisionToWallProc(unit);
			}

			unit.SetCollider(true);

			// while���� ���� ���� �ӵ��� moveSpeed�� ����
			FDebug.Log("This is Fixed");
			unit.rigid.velocity = forward * moveSpeed;
		}
		else // targetPos�� ������ ���
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
			// ���� �� �� ���� ���� �浹�� �� ���ٸ�
			if (firstEnemy == null)
			{
				firstEnemy = collision.transform.GetComponent<Rigidbody>();
				firstEnemy.constraints = RigidbodyConstraints.FreezeRotation;
				enemyDistance = unit.basicCollider.radius + collision.collider.bounds.size.x * 0.5f;
				enemyRayLength = (enemyDistance) * 2 - unit.basicCollider.radius;
			}
			else // ���� �浹�߾��ٸ�
			{
				// �˹� �ڵ�
				// ...
				var unitData = collision.transform.GetComponent<UnitBase>();
				unit.playerData.Attack(unitData);
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

		if (!unit.specialIsReleased)
		{
			level = (int)(currentTime / LevelStandard);
			level = Mathf.Clamp(level, 0, MaxLevel - 1);

			// �ܰ谡 �ٲ���ٸ�
			if (currentLevel != level)
			{
				currentLevel = level;

				if (currentLevel > 0)
				{
					if (curEffect != null)
					{
						unit.rushObjectPool.DeactiveObject(curEffect);
					}

					unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[level - 1].effect);
					curEffect = unit.rushObjectPool.ActiveObject(unit.rushEffects[level - 1].effectPos.position);
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

			// �ʴ� �̵� �ӵ� ���(m/sec)
			moveSpeed = (attackLengthMark) / (Meter * unit.curNode.attackDelay);

			// �ʿ� ���� ����
			forward = unit.transform.forward;
			originPos = unit.transform.position;
			targetPos = originPos + forward * (attackLengthMark / Meter);
			targetMagnitude = (targetPos - originPos).magnitude;
			basicRayLength = moveSpeed * Time.fixedDeltaTime + unit.basicCollider.radius;

			if (curEffect != null)
			{
				unit.rushObjectPool.DeactiveObject(curEffect);
			}

			unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[3].effect);
			curEffect = unit.rushObjectPool.ActiveObject(unit.rushEffects[3].effectPos.position);
			curEffect.transform.rotation = unit.transform.rotation;
			unit.rushObjectPool2 = new ObjectPoolManager<Transform>(unit.rushEffects[4].effect);
			curEffect2 = unit.rushObjectPool2.ActiveObject(unit.rushEffects[4].effectPos.position);
			curEffect2.transform.rotation = unit.transform.rotation;

		}

		if (firstEnemy != null)
		{
			firstEnemy.transform.position = unit.transform.position + forward * enemyDistance;
		}

		currentTime += Time.deltaTime;
	}

	private void CollisionToWallProc(PlayerController unit)
	{
		if(firstEnemy != null) 
		{
			// ���� ���� ���� ������� �ʾ�, �߰� ���ش� �ӽ÷� Attack�� �� �� ȣ�� �ϴ� ������ ��ü
			unit.playerData.Attack(firstEnemy.transform.GetComponent<UnitBase>());
			unit.playerData.Attack(firstEnemy.transform.GetComponent<UnitBase>());
		}

		// ��(��ֹ�)�� �浹������ �ٷ� ���� ����
		unit.ChangeState(PlayerController.PlayerState.AttackAfterDelay);
	}
}
