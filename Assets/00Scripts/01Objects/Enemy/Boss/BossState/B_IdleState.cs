using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.Idle)]
public class B_IdleState : UnitState<BossController>
{
	private float curTime = 0f;

	public override void Begin(BossController unit)
	{
		unit.curState = BossController.BossState.Idle;
	}

	public override void Update(BossController unit)
	{
		curTime += Time.deltaTime;
		if (curTime > unit.skillTypeDelay)
			unit.ChangeState(unit.nextState);
	}


	public override void End(BossController unit)
	{
		curTime = 0f;
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
