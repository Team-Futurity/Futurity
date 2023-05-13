using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Attack)]
public class EnemyAttackState : UnitState<EnemyController>
{
	private float curTime;
	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.atkAnimParam);
		curTime = 0f;
		unit.atkRange.enabled = false;
		unit.atkCollider.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackSetTime, unit, EnemyController.EnemyState.Chase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{
		throw new System.NotImplementedException();
	}
}
