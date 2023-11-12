using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.Idle)]
public class PlayerIdleState : UnitState<PlayerController>
{
	public override void Begin(PlayerController pc)
	{
		pc.rigid.velocity = Vector3.zero;
		pc.animator.SetBool(pc.IsAttackingAnimKey, false);
		pc.rmController.SetRootMotion("Idle");

		if (pc.moveIsPressed)
		{
			pc.ChangeState(PlayerState.Move);
		}

		FDebug.Log("Enter IDle");
	}

	public override void Update(PlayerController pc)
	{
		if (!pc.playerData.isKnockbaking)
		{
			pc.rigid.velocity = Vector3.zero;
		}
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		FDebug.Log("Exit IDle");
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
