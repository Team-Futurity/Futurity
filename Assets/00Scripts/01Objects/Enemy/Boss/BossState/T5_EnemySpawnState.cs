using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T5_EnemySpawn)]
public class T5_EnemySpawnState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T5_EnemySpawn;
		unit.nextState = BossState.Chase;
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);

		if (curTime > unit.curAttackData.attackDelay && !isAttackDelayDone)
		{
			isAttackDelayDone = true;
		}

		if (isAttackDelayDone && !isAttackDone)
		{
			unit.animator.SetTrigger(unit.type5Anim);
			unit.attackTrigger.type5Manager.SpawnEnemy();
			isAttackDone = true;
		}

		if (isAttackDone && curTime > unit.curAttackData.attackDelay + unit.curAttackData.attackSpeed + unit.curAttackData.attackAfterDelay)
		{
			unit.ChangeState(unit.nextState);
		}
	}

	public override void End(BossController unit)
	{
		base.End(unit);
	}
}
