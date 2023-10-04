using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.Idle)]
public class B_IdleState : UnitState<BossController>
{
	public override void Begin(BossController unit)
	{
	}

	public override void End(BossController unit)
	{
	}

	public override void Update(BossController unit)
	{
		
	}

	public override void FixedUpdate(BossController unit)
	{
	}

	public override void OnCollisionEnter(BossController unit, Collision collision)
	{
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
	}
}
