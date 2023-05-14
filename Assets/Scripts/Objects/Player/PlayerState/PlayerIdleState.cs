using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.Idle)]
public class PlayerIdleState : UnitState<PlayerController>
{
	public override void Begin(PlayerController pc)
	{
		pc.rigid.velocity = Vector3.zero;
		return;
	}

	public override void Update(PlayerController pc)
	{
		return;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		return;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		throw new System.NotImplementedException();
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		throw new System.NotImplementedException();
	}
}
