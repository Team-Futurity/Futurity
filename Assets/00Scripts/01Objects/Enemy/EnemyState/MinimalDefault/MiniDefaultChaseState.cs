using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.MiniDefaultChase)]
public class MiniDefaultChaseState : EnemyChaseBaseState
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
			unit.rigid.velocity = Vector3.zero;
			unit.ChangeState(EnemyState.MiniDefaultDelay);
		}
		else if (distance > unit.attackRange)
		{
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}
}
