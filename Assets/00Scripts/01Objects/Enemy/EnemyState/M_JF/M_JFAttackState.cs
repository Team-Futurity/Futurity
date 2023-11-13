using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.M_JFAttack)]
public class M_JFAttackState : EnemyAttackBaseState
{
	private EffectActiveData effectData = new EffectActiveData();

	public M_JFAttackState()
	{
		effectData.activationTime = EffectActivationTime.InstanceAttack;
		effectData.target = EffectTarget.Ground;
		effectData.index = 0;
		effectData.position = new Vector3(0f, 0.6f, 0f);
		effectData.rotation = Quaternion.Euler(new Vector3(0,0,0));
	}

	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		effectData.parent = unit.gameObject;
		unit.currentEffectData = effectData;
	}
}
