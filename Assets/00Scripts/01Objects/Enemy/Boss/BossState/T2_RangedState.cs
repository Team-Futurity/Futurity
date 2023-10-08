using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T2_Ranged)]
public class T2_RangedState : B_PatternBase
{
	private EffectActiveData effectData = new EffectActiveData();
	public T2_RangedState()
	{
		effectData.activationTime = EffectActivationTime.InstanceAttack;
		effectData.target = EffectTarget.Target;
		effectData.index = 0;
		effectData.position = new Vector3(0f, 0f, 0f);
		effectData.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
	}
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.typeCount++;
		unit.curState = BossController.BossState.T2_Ranged;
		effectData.parent = unit.gameObject;
		unit.currentEffectData = effectData;
		if (unit.typeCount >= 5)
			unit.nextPattern = BossController.BossState.T3_Laser;
		else
			unit.nextPattern = BossController.BossState.Chase;

		unit.animator.SetTrigger(unit.type2Anim);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		if (unit.Type2Collider)
			unit.Type2Collider.SetActive(false);
	}
}
