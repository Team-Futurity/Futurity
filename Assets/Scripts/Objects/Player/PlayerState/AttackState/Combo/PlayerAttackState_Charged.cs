using UnityEngine;

[FSMState((int)PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	public static float LengthMarkIncreasing = 200; // �ܰ�� ���� �Ÿ� ������
	public static float AttackSTIncreasing = 1;     // �ܰ�� ���� ���� ������
	public static float LevelStandard = 1;         // �ܰ踦 ���� ����
	public static float initialLevelStandard = 0.5f;
	public static int MaxLevel = 4;                 // �ִ� ���� �ܰ�
	public static float RangeEffectUnitLength = 0.145f; // Range ����Ʈ�� 1unit�� �ش��ϴ� Z�� ũ��
	public static float FlyPower = 45;               // ���� ü�� ��
	public static float WallCollisionDamage = 50f;	// �� �浹 �� ������
	private readonly string KReleaseAnimKey = "KIsReleased";
	private readonly string DashEndAnimKey = "KDashEnded";
	private readonly float Sqrt2;

	// Variables
	private float playerOriginalSpeed;	// ���� �ӵ�(�ӵ� �����)
	private int currentLevel;			// ���� ���� �ܰ�
	private float moveSpeed;            // ������ �ӵ�
	private float attackLengthMark;     // ���������� �̵��� �Ÿ�
	private float attackST;				// ���� ���ݹ���
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
	private Collider firstEnemyCollider;		// ù��°�� �浹�� ���� �ݶ��̴�
	private UnitBase firstEnemyData;			// ù��°�� �浹�� ���� ������
	private float enemyDistance;				// ù��°�� �浹�� ������ �Ÿ�
	private Vector3 groundPos;          // Enemy�� ü�� ���� ��ǥ
	private float originScale;		// Player�� ���� �ݶ��̴� ũ��

	// Trigger
	public bool isReleased; // ���� ��ư�� Release�Ǹ� true
	private bool isEnd;     // ���� ���μ���(�̵�)�� ����Ǿ��°�
	private bool isLanding;	// Enemy�� �������̶��

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

			// Enemy �ൿ ���� ����(XZ)
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

		// ������ Ray
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * ((unit.curNode.attackLengthMark + currentLevel * LengthMarkIncreasing) / (PlayerController.cm2m * unit.curNode.attackDelay) * Time.fixedDeltaTime + unit.basicCollider.radius), UnityEngine.Color.red);
		FDebug.DrawRay(unit.transform.position, unit.transform.forward * rayLength, UnityEngine.Color.blue);

		if (!isReleased) { return; }
		if (isEnd) // ������ ������, FirstEnemy�� �־�����
		{
			// �߷� ����
			firstEnemy.AddForce(Physics.gravity, ForceMode.Force); 

			// ���߿��� ���� �ߴ��� �Ǻ� 
			if (firstEnemy.velocity.magnitude < 0.1f && !isLanding)
			{
				//DownAttack();
			}

			// ���� �� �߶�������
			if (firstEnemy.velocity.magnitude < 0.3f && isLanding)
			{
				EnemyLanding();
			}
			return;
		}

		// ���� �� ��ġ���� ���� ��ġ�� ���ϴ� ������ ũ�Ⱑ targetMagnitude���� �۰�
		if ((unit.transform.position - originPos).magnitude < targetMagnitude)
		{
			// �� ���� ó��(���� �����ϸ� OnCollisionEnter������ Wall üũ�κа� ��ĥ ��)
			unit.SetCollider(false);

			// Collision�������� ������ �κ��� �޲� Ray����
			// ray�� ���̴� ���� ���ǰ� �ʿ�������...?
			if (Physics.Raycast(unit.transform.position, forward, out hit, rayLength, wallLayer))
			{
				/*unit.rushObjectPool = new ObjectPoolManager<Transform>(unit.rushEffects[5].effect);
				curEffect = unit.rushObjectPool.ActiveObject(point.point);*/
				unit.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Target, hit.point, Quaternion.LookRotation(hit.normal), null, 1);
				unit.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Caster, unit.rushEffects[0].effectPos.position, Quaternion.LookRotation(hit.normal), unit.gameObject);
				
				CollisionToWallProc(unit);
			}

			unit.SetCollider(true);

			// �̵�
			unit.rigid.velocity = forward * moveSpeed;

			if (firstEnemy != null)
			{
				firstEnemy.transform.position = unit.transform.position + forward * enemyDistance;
			}
		}
		else // targetPos�� ������ ���
		{
			// ��ġ ����
			unit.transform.position = targetPos;

			if (currentLevel > 0)
			{
				unit.animator.SetTrigger(DashEndAnimKey);
			}

			if (firstEnemy != null)
			{
				// Enemy ��ġ ����
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
			// ���� �� �� ���� ���� �浹�� �� ���ٸ�
			if (firstEnemy == null)
			{
				// enemy�� �ݶ��̴� ���� ũ�� ���
				Vector3 localScale = collision.transform.localScale;
				float halfSize = Vector3.Dot(localScale, (collision.transform.position - unit.transform.position).normalized);

				// Enemy �⺻ ����
				firstEnemy = collision.rigidbody;
				firstEnemy.constraints = RigidbodyConstraints.FreezeRotation;
				firstEnemyCollider = collision.collider;
				firstEnemyCollider.enabled = false;
				firstEnemyData = collision.transform.GetComponent<UnitBase>();
				enemyDistance = unit.basicCollider.radius + halfSize * 0.5f;
				enemyRayLength = (enemyDistance) * 2 - unit.basicCollider.radius;

				// �ݶ��̴� ����
				unit.basicCollider.radius = originScale + 2 * halfSize;

				// �浹 ������ ó��
				unit.playerData.Attack(firstEnemyData, attackNode.attackST);
			}
			else if(collision.body != firstEnemy) // ���� �浹�߰�, �װ� ó�� �ε�ģ ���� �ƴ϶��
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

			// �ܰ谡 �ٲ���ٸ�
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

			// �����̶��
			if(currentLevel > 0)
			{
				// ���� ����Ʈ�� 1�ܰ� �̻󿡼��� ����

				// Remove Charge Effect
				unit.effectManager.RemoveEffect(chargeEffectKey, null, true);
				unit.effectManager.RemoveEffect(rangeEffect);

				// Active Move Effects
				rushBodyEffect = unit.effectManager.ActiveEffect(EffectActivationTime.MoveWhileAttack, EffectTarget.Caster);
				unit.effectManager.RegisterTracking(rushBodyEffect, unit.rushEffects[2].effectPos);
				rushGroundEffect = unit.effectManager.ActiveEffect(EffectActivationTime.MoveWhileAttack, EffectTarget.Ground);
				unit.effectManager.RegisterTracking(rushGroundEffect, unit.transform);

				// ���� ó��
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

		// ��(��ֹ�)�� �浹������ �ٷ� ���� ����
		unit.ChangeState(PlayerState.AttackAfterDelay);
	}

	private void CalculateRushData(PlayerController unit)
	{
		// ���� ����
		attackST = unit.curNode.attackST + currentLevel * AttackSTIncreasing;

		// �ʴ� �̵� �ӵ� ���(m/sec)
		moveSpeed = (attackLengthMark * PlayerController.cm2m) / (unit.curNode.attackDelay);

		// �ʿ� ���� ����
		forward = unit.transform.forward;
		originPos = unit.transform.position;
		targetPos = originPos + forward * (attackLengthMark * PlayerController.cm2m);
		targetMagnitude = (targetPos - originPos).magnitude;
		basicRayLength = moveSpeed * Time.fixedDeltaTime + Sqrt2 * unit.basicCollider.radius;
	}

	private void RemoveEffect(PlayerController unit, EffectKey effect)
	{
		// key�� �ְ� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִٸ�
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
		// Enemy �ʱ� ��ġ�� Ground�� ����
		groundPos = firstEnemy.transform.position;

		// Enemy �ൿ ���� ����(XZ)
		firstEnemy.constraints = RigidbodyConstraints.FreezeAll ^ RigidbodyConstraints.FreezePositionY;
		firstEnemyCollider.enabled = true;

		// Enemy ������ 
		firstEnemy.AddForce(Vector3.up * FlyPower, ForceMode.VelocityChange);
		pc.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Target, firstEnemy.transform.position);

		// ������ �������� �˸�
		isEnd = true;

		pc.basicCollider.radius = originScale;
		pc.rigid.velocity = Vector3.zero;
	}

	public void DownAttack()
	{
		// �ӵ� �ʱ�ȭ �� �Ʒ��� ���� ����
		firstEnemy.velocity = Vector3.zero;
		firstEnemy.AddForce(Vector3.down * FlyPower, ForceMode.VelocityChange);

		// Landing ���°� ���� �˸�
		isLanding = true;
	}

	public void EnemyLanding()
	{
		// ����Ʈ ���
		pc.effectManager.ActiveEffect(EffectActivationTime.AfterDoingAttack, EffectTarget.Ground, groundPos, null, null, 0, currentLevel - 1);

		// ������ ����ȭ
		firstEnemy.transform.position = groundPos;

		// ������ ó��
		pc.playerData.Attack(firstEnemyData, attackST);

		// State Change
		pc.ChangeState(PlayerState.AttackAfterDelay);
	}
}
