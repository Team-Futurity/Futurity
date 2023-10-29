using UnityEngine;

[FSMState((int)PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	public static ChargeIncreases[] IncreasesByLevel;
	public static int MaxLevel = 4;                 // �ִ� ���� �ܰ�
	public static float RangeEffectUnitLength = 0.145f; // Range ����Ʈ�� 1unit�� �ش��ϴ� Z�� ũ��
	public static float FlyPower = 45;               // ���� ü�� ��
	public static float WallCollisionDamage = 50f;  // �� �浹 �� ������
	public static float MoveSpeedInCharging = 0.5f;
	public static ChargeCollisionData ChargeCollisionData;
	public static GameObject ChargeCollisionEffect;
	private readonly string KReleaseAnimKey = "KIsReleased";
	private readonly string DashEndAnimKey = "KDashEnded";
	private readonly float Sqrt2;

	// Variables
	private float playerOriginalSpeed;	// ���� �ӵ�(�ӵ� �����)
	private int currentLevel;			// ���� ���� �ܰ�
	private float moveSpeed;            // ������ �ӵ�
	private float attackLengthMark;     // ���������� �̵��� �Ÿ�
	private float attackKnockback;     // ���������� �̵��� �Ÿ�
	private float attackST;				// ���� ���ݹ���
	private float targetMagnitude;		// originPos���� targetPos�� ���ϴ� ������ ũ��^2
	private Vector3 forward;			// �ü� ����
	private Vector3 originPos;          // ���� �� ��ġ
	private Vector3 targetPos;          // ��ǥ ��ġ
	private float originScale;		// Player�� ���� �ݶ��̴� ũ��

	// Trigger
	public bool isReleased; // ���� ��ư�� Release�Ǹ� true

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
		rangeEffect.EffectObject.transform.localScale = new Vector3(rangeEffect.EffectObject.transform.localScale.x, rangeEffect.EffectObject.transform.localScale.y, RangeEffectUnitLength * unit.curNode.attackLengthMark * MathPlus.cm2m);

		pc = unit;
		unit.playerData.EnableAttackTime();
		unit.playerData.EnableAttackTiming();
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);

		unit.rigid.velocity = Vector3.zero;

		isReleased = false;
		unit.specialIsReleased = false;
		unit.playerData.status.GetStatus(StatusType.SPEED).SetValue(playerOriginalSpeed);

		RemoveEffects(unit);

		pc.basicCollider.radius = originScale;
		pc.rigid.velocity = Vector3.zero;
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);

		// ������ Ray
		FDebug.DrawLine(unit.transform.position, targetPos, Color.red);

		if (!isReleased) { SetPostionChargeEffect(unit);  return; }

		// ���� �� ��ġ���� ���� ��ġ�� ���ϴ� ������ ũ�Ⱑ targetMagnitude���� �۰�
		if ((unit.transform.position - originPos).magnitude < targetMagnitude)
		{
			// �̵�
			unit.rigid.velocity = forward * moveSpeed;
		}
		else // targetPos�� ������ ���
		{
			// ��ġ ����
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

				unitData = collision.transform.GetComponent<UnitBase>();
				unitData.Knockback(knockbackDir, attackKnockback * 2);

				DamageInfo wallDamageInfo = new DamageInfo(unit.playerData, unitData, unit.curNode.attackST);
				gotoWall.RunCollision(ChargeCollisionData, wallDamageInfo, collisionToWallEffectPoolManager, collision.rigidbody, unit.camera);
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
				rangeEffect.EffectObject.transform.localScale = Vector3.Lerp(rangeEffect.EffectObject.transform.localScale, maxRangeEffectScale, spendingTime / Time.deltaTime);
			}

			// �ܰ谡 �ٲ���ٸ�
			if (currentLevel != level)
			{
				currentLevel = level;

				unit.effectController.SetEffectLevel(ref chargeEffectKey, currentLevel);

				FDebug.Log(currentLevel + "_" + chargeEffectKey.EffectObject.name + "_" + chargeEffectKey.IsTrackingEffect());

				unit.animator.SetInteger(unit.currentAttackAnimKey, currentLevel);
				rangeEffect.EffectObject.transform.localScale = new Vector3(rangeEffect.EffectObject.transform.localScale.x, rangeEffect.EffectObject.transform.localScale.y, RangeEffectUnitLength * attackLengthMark * MathPlus.cm2m);
				attackLengthMark = unit.curNode.attackLengthMark + IncreasesByLevel[currentLevel].LengthMarkIncreasing;
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

			// ���� ó��
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

		// ��(��ֹ�)�� �浹������ �ٷ� ���� ����
		NextAttackState(unit, PlayerState.AttackAfterDelay);
	}

	private void CalculateRushData(PlayerController unit)
	{
		// ���� �� ������
		attackST = unit.curNode.attackST + IncreasesByLevel[currentLevel].AttackSTIncreasing;
		attackKnockback = unit.curNode.attackKnockback + IncreasesByLevel[currentLevel].KnockbackIncreasing;

		// �ʴ� �̵� �ӵ� ���(m/sec)
		moveSpeed = (attackLengthMark * MathPlus.cm2m) / (unit.curNode.attackDelay);

		// �ʿ� ���� ����
		forward = unit.transform.forward;
		originPos = unit.transform.position;
		targetPos = originPos + forward * (attackLengthMark * MathPlus.cm2m);
		targetMagnitude = (targetPos - originPos).magnitude;
		basicRayLength = moveSpeed * Time.fixedDeltaTime + Sqrt2 * unit.basicCollider.radius;
	}

	private void RemoveEffect(PlayerController unit, EffectKey effect)
	{
		// key�� �ְ� ������Ʈ�� Ȱ��ȭ�Ǿ� �ִٸ�
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

	private void SetPostionChargeEffect(PlayerController unit)
	{
		Vector3 midPosition = (unit.hands[0].transform.position + unit.hands[1].transform.position) * 0.5f;
		chargeEffectPos.transform.position = midPosition;
	}
}
