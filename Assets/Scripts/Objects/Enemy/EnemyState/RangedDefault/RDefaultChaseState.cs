using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.RDefaultChase)]
public class RDefaultChaseState : UnitState<EnemyController>
{
	private float curTime;

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.isChasing = true;
		curTime = 0;
	}

	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;
		unit.transform.LookAt(unit.target.transform.position);

		float distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);

		if (distance < unit.rangedAttackDistance - 1.0f)
		{
			FDebug.Log("1");
			unit.transform.position = Vector3.MoveTowards(unit.transform.position,
				unit.RangedBackPos.transform.position,
				unit.enemyData.Speed * Time.deltaTime);
		}
		else if (distance > unit.rangedAttackDistance - 1.0f && distance < unit.rangedAttackDistance + 1.0f)
		{
			FDebug.Log("22222");
			if (curTime != 0)
				curTime = 0;
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.DelayChangeState(curTime, unit.attackSetTime, unit, EnemyController.EnemyState.RDefaultAttack);
		}
		else if (distance > unit.rangedAttackDistance + 1.0f)
		{
			FDebug.Log("3333333333333");
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
