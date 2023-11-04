using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.Idle)]
public class B_IdleState : BossStateBase
{
	private float idleDelay = 0.5f;

	private bool isDelayDone = false;

	public override void Begin(BossController unit)
	{
		unit.curState = BossState.Idle;
	}

	public override void Update(BossController unit)
	{
		base.Update(unit);
		if (curTime > idleDelay && !isDelayDone)
		{
			distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);
			if (distance > unit.chaseDistance)
				unit.nextState = BossState.Chase;
			else
				unit.activeDataSO.SetRandomNextState(unit);

			isDelayDone = true;
		}

		if(isDelayDone)
			unit.ChangeState(unit.nextState);
	}

	public override void End(BossController unit)
	{
		base.End(unit);

		isDelayDone = false;
		unit.previousState = BossState.Idle;
	}
}
