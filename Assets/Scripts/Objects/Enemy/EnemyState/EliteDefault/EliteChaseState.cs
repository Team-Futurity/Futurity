using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.EliteRangedAttack);
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
