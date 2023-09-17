using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[FSMState((int)EnemyController.EnemyState.EliteMeleeAttack)]
public class EliteMeleeAttackState : EnemyAttackBaseState
{
	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		unit.rigid.velocity = Vector3.zero;
		unit.navMesh.enabled = true;
		unit.atkCollider.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.EliteDefaultChase);
	}

	public override void End(EnemyController unit)
	{

		base.End(unit);
		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}
}
