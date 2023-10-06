using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T5_EnemySpawn)]
public class T5_EnemySpawnState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossController.BossState.T5_EnemySpawn;
		unit.nextPattern = BossController.BossState.Chase;
		unit.animator.SetTrigger(unit.type5Anim);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.cur5Time = 0f;
		unit.isActivateType5 = false;
	}
}
