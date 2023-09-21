using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultAttack)]
public class MDefaultAttackState : EnemyAttackBaseState
{
	public EffectActiveData atk1 = new EffectActiveData();

	public MDefaultAttackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Caster;
		atk1.position = new Vector3(0, 1.0f, 0);
		atk1.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack begin");
		base.Begin(unit);

		unit.currentEffectData = atk1;
		unit.navMesh.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.MDefaultAttack2nd);
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack End");

		base.End(unit);

		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		//FDebug.Log("MDefault Attack Trigger");
		base.OnTriggerEnter(unit, other);
	}
}
