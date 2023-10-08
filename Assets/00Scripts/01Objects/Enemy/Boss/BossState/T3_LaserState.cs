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
		unit.SetEffectData(unit.Type3Colliders, EffectActivationTime.InstanceAttack, EffectTarget.Ground);

		if (unit.curPhase == Phase.Phase2 || unit.curPhase == Phase.Phase4)
		{
			if (unit.isActivateType5)
				unit.nextPattern = BossController.BossState.T5_EnemySpawn;
		}
		else
			unit.nextPattern = BossController.BossState.Chase;
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
		unit.DeActiveAttacks(unit.Type3Colliders);
	}
}
