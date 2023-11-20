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

		if (pc.moveIsPressed && !pc.lockAllInput)
		{
			pc.ChangeState(PlayerState.Move);
		}
	}

	public override void Update(PlayerController pc)
	{
		
	}

	public override void FixedUpdate(PlayerController unit)
	{
		if (!unit.playerData.isKnockbaking)
		{
			unit.rigid.velocity = Vector3.zero;
		}
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
