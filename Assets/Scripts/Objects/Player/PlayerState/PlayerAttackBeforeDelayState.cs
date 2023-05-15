using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerController.PlayerState.AttackDelay)]
public class PlayerAttackBeforeDelayState : UnitState<PlayerController>
{
	// Animation Key
	protected readonly string IsAttackingAnimKey = "IsAttacking";
	protected readonly string AttackTriggerAnimKey = "AttackTrigger";

	// etc
	private float currentTime;
	private Transform effect;
	//private CameraController cam;
	protected AttackNode attackNode;

	public override void Begin(PlayerController pc)
	{
		//pc.rigid.velocity = Vector3.zero;

		/*if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		string key = pc.currentAttackState == PlayerState.NormalAttack ? pc.ComboAttackAnimaKey : pc.ChargedAttackAnimaKey;

		pc.animator.SetBool(IsAttackingAnimKey, true);
		pc.animator.SetFloat(key, pc.curNode.animFloat);
		pc.animator.SetTrigger(AttackTriggerAnimKey);
		pc.curNode.Copy(pc.curNode);
		attackNode = pc.curNode;
		currentTime = 0;

		pc.glove.SetActive(true);

		FDebug.Log("CurrentState : AttackBefore");
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime > attackNode.attackDelay)
		{
			pc.ChangeState(pc.currentAttackState);
		}
		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{

	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
