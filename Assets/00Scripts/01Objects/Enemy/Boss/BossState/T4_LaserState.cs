using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T4_Laser)]
public class T4_LaserState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossController.BossState.T4_Laser;
		unit.SetEffectData(unit.Type4Colliders, EffectActivationTime.MoveWhileAttack, EffectTarget.Caster, false);

		unit.nextPattern = unit.afterType467Pattern;
		unit.animator.SetTrigger(unit.type4Anim);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.DeActiveAttacks(unit.Type4Colliders);
	}
}
