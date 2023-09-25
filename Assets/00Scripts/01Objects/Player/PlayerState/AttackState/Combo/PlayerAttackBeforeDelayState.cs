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
		float range = isCombo 
			? pc.autoLength * MathPlus.cm2m 
			: (attackNode.attackLengthMark + (PlayerAttackState_Charged.MaxLevel - 1) * PlayerAttackState_Charged.LengthMarkIncreasing) * MathPlus.cm2m;
		pc.autoTargetCollider.truncatedCollider.enabled = true;
		pc.attackCollider.truncatedCollider.enabled = true;
		pc.autoTargetCollider.SetCollider(pc.autoAngle, range);
		pc.attackCollider.SetCollider(attackNode.attackAngle, attackNode.attackLength * MathPlus.cm2m);

		targets.Clear();

		pc.playerData.EnableAttackTime();

		// ohter Setting
		pc.glove.SetActive(true);

		// sound
		if (isCombo)
		{
			AudioManager.instance.PlayOneShot(attackNode.attackSound, pc.transform.position);
		}
	}

	public override void Update(PlayerController pc)
	{
		base.Update(pc);

		if (targets.Count > 0)
		{
			bool isMove = AutoTarget.Instance.AutoTargetProcess(targets, pc.gameObject, pc.attackCollider, pc.autoAngle, pc.moveMargin, pc.moveTime);
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

		pc.attackCollider.truncatedCollider.enabled = false;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		if (other.CompareTag(unit.EnemyTag))
		{
			if(unit.attackCollider.IsInCollider(other.gameObject) || unit.autoTargetCollider.IsInCuttedCollider(other.gameObject))
			{
				targets.Add(other.gameObject);
			}
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
