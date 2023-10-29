using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[FSMState((int)EnemyState.RDefaultChase)]
public class RDefaultChaseState : EnemyChaseBaseState
{
	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
	}

	public override void End(EnemyController unit)
	{
		base.End(unit);
	}
	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		unit.transform.LookAt(unit.target.transform.position);

		if (distance < unit.attackRange)
		{
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.DelayChangeState(curTime, 0.5f, unit, EnemyState.RDefaultAttack);
		}
		else if (distance > unit.attackRange && distance < targetDistance)
		{
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
		else if (distance > targetDistance)
			unit.ChangeState(EnemyState.Default);
	}
}
