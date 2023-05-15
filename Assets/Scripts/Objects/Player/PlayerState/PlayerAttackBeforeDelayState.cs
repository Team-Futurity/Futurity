using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

//[FSMState((int)PlayerController.PlayerState.Attack)]
public class PlayerAttackBeforeDelayState : UnitState<PlayerController>
{
	// Animation Key
	protected readonly string IsAttackingAnimKey = "IsAttacking";
	protected readonly string AttackTypeAnimaKey = "Combo";

	// etc
	private float currentTime;
	private Transform effect;
	//private CameraController cam;
	protected AttackNode attackNode;

	public override void Begin(PlayerController pc)
	{
		/*if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		pc.animator.SetBool(IsAttackingAnimKey, true);
		pc.animator.SetFloat(AttackTypeAnimaKey, pc.curNode.animFloat);
		pc.curNode.Copy(pc.curNode);
		attackNode = pc.curNode;
		currentTime = 0;

		pc.glove.SetActive(true);
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime > attackNode.attackSpeed * animRatio)
		{
			pc.ChangeState(PlayerState.AttackDelay);
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
