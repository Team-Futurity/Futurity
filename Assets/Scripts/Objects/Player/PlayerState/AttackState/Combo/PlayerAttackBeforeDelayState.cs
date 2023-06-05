using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerController.PlayerState.AttackDelay)]
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

		// sound
		if (isCombo)
		{
			AudioManager.instance.PlayOneShot(attackNode.attackSound, pc.transform.position);
		}

		// autoTargetting
		pc.autoTargetCollider.radiusCollider.enabled = true;
		pc.autoTargetCollider.SetCollider(360, attackNode.attackLength * cm2m);
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
		pc.autoTargetCollider.radiusCollider.enabled = false;
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
