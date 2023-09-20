using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.EliteDefaultChase)]
public class EliteChaseState : EnemyChaseBaseState
{
	private float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		unit.transform.LookAt(unit.target.transform.position);


		if (distance < unit.attackRange * 0.5f)
		{
			unit.ChangeState(EnemyController.EnemyState.EliteMeleeAttack);
		}
		else if (distance < unit.attackRange)
		{
			unit.ChangeState(EnemyController.EnemyState.EliteRangedAttack);
		}
		else if (distance > unit.attackRange)
		{
			unit.navMesh.enabled = true;
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		base.End(unit);
		curTime = 0f;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{
	}
}
