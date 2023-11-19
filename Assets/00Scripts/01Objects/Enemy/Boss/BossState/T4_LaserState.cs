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

		unit.SetListEffectData(unit.attackEffectDatas, unit.attackTrigger.type4Colliders, EffectActivationTime.AttackReady, EffectTarget.Caster, false);
		if (unit.attackEffectDatas.Count > 3)
		{
			unit.attackEffectDatas[0].position.x += 15;
			unit.attackEffectDatas[0].position.z += 34;
			unit.attackEffectDatas[1].position.x += 15;
			unit.attackEffectDatas[1].position.z -= 26;
			unit.attackEffectDatas[2].position.x -= 15;
			unit.attackEffectDatas[2].position.z += 30;
			unit.attackEffectDatas[3].position.x -= 15;
			unit.attackEffectDatas[3].position.z -= 30;
		}
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
