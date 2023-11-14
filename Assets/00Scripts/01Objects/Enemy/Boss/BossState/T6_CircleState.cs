using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T6_Circle)]
public class T6_CircleState : B_PatternBase
{
	private GameObject T6Manager;
	private T6_AttackProcess attackProcess;

	public T6_CircleState()
	{
		T6Manager = new GameObject("T6 Manager");
		T6Manager.AddComponent<T6_AttackProcess>();
		attackProcess = T6Manager.GetComponent<T6_AttackProcess>();
	}

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T6_Circle;

		unit.attackTrigger.SetT6RandomVector(unit);
		unit.SetListEffectData(unit.floorEffectDatas, unit.attackTrigger.type6Colliders, EffectActivationTime.AttackReady, EffectTarget.Ground, false);
		foreach (var o in unit.floorEffectDatas)
		{
			o.position = new Vector3(0f, 1.0f, 0f);
			o.rotation = Quaternion.Euler(new Vector3(-90f, 0, 0));
		}
		unit.SetListEffectData(unit.attackEffectDatas, unit.attackTrigger.type6Colliders, EffectActivationTime.MoveWhileAttack, EffectTarget.Target, false);

		attackProcess.T6Setting(unit.floorEffectDatas, unit.attackEffectDatas, unit.attackTrigger.type6Colliders, unit.flooringTiming, unit.atkEffectTiming, unit.atktTiming, unit.deActiveTiming, unit.attackSpeed, null, unit);
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
			unit.animator.SetTrigger(unit.type6Anim);
			attackProcess.StartProcess();
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
		unit.attackTrigger.DeActiveListAttacks(unit.floorEffectDatas, unit.attackTrigger.type6Colliders);
		unit.attackTrigger.DeActiveListAttacks(unit.attackEffectDatas, unit.attackTrigger.type6Colliders);
	}
}
