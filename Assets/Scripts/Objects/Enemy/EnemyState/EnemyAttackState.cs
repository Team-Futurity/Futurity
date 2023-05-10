using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Attack)]
public class EnemyAttackState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger("Attack");
		unit.attackAnimTime = unit.enemyData.attackDelay;
		unit.attackCurTime = 0f;
		unit.atkRange.enabled = false;
		unit.atkCollider.enabled = true;

		FDebug.Log("Enemy Attack");
	}

	public override void Update(EnemyController unit)
	{
		unit.attackCurTime += Time.deltaTime;

		if (unit.attackCurTime > unit.attackAnimTime)
		{
			if (!unit.IsCurrentState(EnemyController.EnemyState.Chase))
			{
				unit.ChangeState(EnemyController.EnemyState.Chase);
			}
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.atkCollider.enabled = false;
		unit.attackCurTime = 0f;

		FDebug.Log("Enemy Attack End");
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

}
