using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T1_Melee)]
public class T1_MeleeState : B_PatternBase
{
	private EffectActiveData effectData = new EffectActiveData();
	public T1_MeleeState()
	{
		effectData.activationTime = EffectActivationTime.InstanceAttack;
		effectData.target = EffectTarget.Caster;
		effectData.index = 0;
		effectData.position = new Vector3(0f, 1f, 0f);
		effectData.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
	}

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T1_Melee;
		effectData.parent = unit.gameObject;
		unit.currentEffectData = effectData;
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
			unit.animator.SetTrigger(unit.type1Anim);
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
