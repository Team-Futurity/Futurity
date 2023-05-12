using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.RDefaultChase)]
public class RDefaultChaseState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.atkRange.enabled = true;
		unit.isChasing = true;
	}

	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;
		unit.transform.LookAt(unit.target.transform.position);

		float distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);

		if (distance < unit.distance - 1.0f)
		{
			unit.transform.position = Vector3.MoveTowards(unit.transform.position,
			unit.RangedBackPos.transform.position,
		unit.enemyData.Speed * Time.deltaTime);
		}
		else if (distance == unit.distance)
		{
			unit.ChangeState(EnemyController.EnemyState.RDefaultAttack);
		}
		else if (distance > unit.distance)
		{
			unit.transform.position += unit.transform.forward * unit.enemyData.Speed * Time.deltaTime;
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.isChasing = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}
}
