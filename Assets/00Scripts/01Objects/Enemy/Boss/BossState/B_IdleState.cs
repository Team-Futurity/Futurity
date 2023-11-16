using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.Idle)]
public class B_IdleState : BossStateBase
{
	private float idleDelay = 0.5f;
	public override void Begin(BossController unit)
	{
		unit.curState = BossState.Idle;

		unit.cube.SetActive(true);

		if (unit.nextState == unit.curState)
			unit.nextState = BossState.Chase;
	}

	public override void Update(BossController unit)
	{
		base.Update(unit);
		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);
		if (curTime > idleDelay && !isAttackDelayDone)
		{
			if (distance > unit.chaseDistance)
				unit.nextState = BossState.Chase;
			else
				unit.activeDataSO.SetRandomNextState(unit);

			isAttackDelayDone = true;
		}

		if(isAttackDelayDone)
			unit.ChangeState(unit.nextState);
	}

	public override void End(BossController unit)
	{
		isAttackDelayDone = false;
		unit.cube.SetActive(false);
	}
}
