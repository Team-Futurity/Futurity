using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T7_Trap)]
public class T7_TrapState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossController.BossState.T7_Trap;
		unit.nextPattern = unit.afterType467Pattern;
		unit.animator.SetTrigger(unit.type7Anim);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.isActivateType467 = false;
	}
}
