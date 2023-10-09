using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T1_Melee)]
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
		unit.typeCount++;
		unit.curState = BossController.BossState.T1_Melee;
		unit.extraAttackValue = unit.bossData.status.GetStatus(StatusType.Type1_Attack_Point).GetValue();
		effectData.parent = unit.gameObject;
		unit.currentEffectData = effectData;
		if (unit.typeCount >= 5)
			unit.nextPattern = BossController.BossState.T3_Move;
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
