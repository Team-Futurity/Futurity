using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T1_Melee)]
public class T1_MeleeState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.typeCount++;
		unit.curState = BossController.BossState.T1_Melee;
		if (unit.typeCount >= 5)
			unit.nextPattern = BossController.BossState.T3_Laser;
		else
			unit.nextPattern = BossController.BossState.Chase;

		unit.animator.SetTrigger(unit.type1Anim);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		if (unit.Type1Collider)
			unit.Type1Collider.SetActive(false);
	}
}
