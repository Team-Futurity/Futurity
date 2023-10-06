using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T3_Laser)]
public class T3_LaserState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossController.BossState.T3_Laser;
		unit.animator.SetTrigger(unit.type3Anim);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.typeCount = 0;
		if (unit.Type3Collider)
			unit.Type3Collider.SetActive(false);
	}
}
