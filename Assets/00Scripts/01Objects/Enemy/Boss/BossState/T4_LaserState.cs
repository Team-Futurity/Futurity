using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T4_Laser)]
public class T4_LaserState : B_PatternBase
{
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T4_Laser;
		unit.SetListEffectData(unit.attackEffectDatas, unit.attackTrigger.type4Colliders, EffectActivationTime.MoveWhileAttack, EffectTarget.Caster, false);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);

		if (curTime > unit.curAttackData.attackDelay && !isAttackDelayDone)
		{
			unit.transform.LookAt(unit.target.transform.position);
			isAttackDelayDone = true;
		}

		if (isAttackDelayDone && !isAttackDone)
		{
			ActiveAnimProcess(unit, unit.type4Anim);
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
