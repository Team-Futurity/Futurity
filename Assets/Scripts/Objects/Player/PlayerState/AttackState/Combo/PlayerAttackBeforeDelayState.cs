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

		// sound
		if (isCombo)
		{
			AudioManager.instance.PlayOneShot(attackNode.attackSound, pc.transform.position);
		}

		// autoTargetting
		float range = isCombo ? attackNode.attackLength * PlayerController.cm2m : (attackNode.attackLengthMark + (PlayerAttackState_Charged.MaxLevel - 1) * PlayerAttackState_Charged.LengthMarkIncreasing) * PlayerController.cm2m;
		pc.autoTargetCollider.radiusCollider.enabled = true;
		pc.autoTargetCollider.SetCollider(360, range);
		targets.Clear();

		// ohter Setting
		pc.glove.SetActive(true);
	}

	public override void Update(PlayerController pc)
	{
		base.Update(pc);

		if (targets.Count > 0)
		{
			AutoTarget.Instance.TurnToNearstObject(targets, pc.gameObject);
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
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		if (other.CompareTag(unit.EnemyTag))
		{
			targets.Add(other.gameObject);
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
