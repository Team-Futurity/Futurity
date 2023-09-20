using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultAttack)]
public class MDefaultAttackState : EnemyAttackBaseState
{
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack begin");
		base.Begin(unit);

		//unit.atkCollider.enabled = true;
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
