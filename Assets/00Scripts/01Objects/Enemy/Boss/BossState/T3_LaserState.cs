using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T3_Laser)]
public class T3_LaserState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
	}
	public override void Update(BossController unit)
	{

	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.typeCount = 0;
	}
}
