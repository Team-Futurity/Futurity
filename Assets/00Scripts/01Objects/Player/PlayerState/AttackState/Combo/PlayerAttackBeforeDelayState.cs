using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.AttackDelay)]
public class PlayerAttackBeforeDelayState : PlayerComboAttackState
{
	// Animation Key
	protected readonly string AttackTriggerAnimKey = "AttackTrigger";

	// etc
	private Transform effect;
	//private CameraController cam;
	private List<GameObject> targets = new List<GameObject>();
	public int GetTargetCount() => targets.Count;

	// collider
	private ColliderBase attackColliderData;
	private ColliderBase autotargetColliderData;

	// switch
	private bool isAutoTargeted;

	public override void Begin(PlayerController pc)
	{
		base.Begin(pc);

		//pc.rigid.velocity = Vector3.zero;

		/*if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		// animation
		bool isCombo = pc.currentAttackState == PlayerState.NormalAttack;
		pc.currentAttackAnimKey = isCombo ? pc.ComboAttackAnimaKey : pc.ChargedAttackAnimaKey;

		if (!pc.animator.GetBool(pc.IsAttackingAnimKey))
		{
			pc.animator.SetTrigger(AttackTriggerAnimKey);
		}

		pc.animator.SetBool(pc.IsAttackingAnimKey, true);
		pc.animator.SetInteger(pc.currentAttackAnimKey, pc.curNode.animInteger);
		pc.rmController.SetRootMotion("Attack");

		// autoTargetting
		float range = 
			isCombo 
			? 
			pc.autoLength * MathPlus.cm2m 
			: 
			(attackNode.attackLengthMark + PlayerAttackState_Charged.IncreasesByLevel[PlayerAttackState_Charged.MaxLevel - 1].LengthMarkIncreasing) * MathPlus.cm2m;

		// Collider
		pc.attackColliderChanger.EnableCollider(attackNode.attackColliderType, out attackColliderData);
		pc.autoTargetColliderChanger.EnableCollider(attackNode.attackColliderType, out autotargetColliderData);
		attackColliderData.SetCollider(attackNode.attackAngle, attackNode.attackLength * MathPlus.cm2m);
		autotargetColliderData.SetCollider(pc.autoAngle, range);

		isAutoTargeted = false;

		targets.Clear();

		pc.playerData.EnableAttackTime();

		// ohter Setting
		pc.SetGauntlet(true);
		pc.sariObject.OnDelayPreMove();

		// sound
		if (isCombo)
		{
			var asset = attackNode.GetAttackAsset(pc.partSystem.GetEquiped75PercentPointPartCode());

			if(asset != null)
			{
				AudioManager.Instance.PlayOneShot(asset.attackSound, pc.transform.position);
			}
		}
	}

	public override void Update(PlayerController pc)
	{
		base.Update(pc);

		if (targets.Count > 0 && !isAutoTargeted)
		{
			ColliderBase collider = pc.attackColliderChanger.GetCollider(attackNode.attackColliderType);
			bool isMove = AutoTarget.Instance.AutoTargetProcess(targets, pc.gameObject, collider, pc.autoAngle, pc.moveMargin, pc.moveTime, !pc.curNode.ignoresAutoTargetMove);
			isAutoTargeted = true;
			// 오토타겟 이동 2안) /*if (isMove) { pc.ResetCombo(); pc.StartNextComboAttack(PlayerInputEnum.NormalAttack, PlayerState.NormalAttack); Begin(pc); }*/
		}

		if (currentTime >= attackNode.attackDelay)
		{
			NextAttackState(pc, pc.currentAttackState);
		}
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		base.End(pc);

		attackColliderData.SetColliderActivation(false);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		if (other.CompareTag(unit.EnemyTag))
		{
			if(attackColliderData.IsInCollider(other.gameObject) || autotargetColliderData.IsInCuttedCollider(other.gameObject, attackNode.attackColliderType != ColliderType.Capsule))
			{
				targets.Add(other.gameObject);
			}
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
