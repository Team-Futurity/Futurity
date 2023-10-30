using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[FSMState((int)EnemyState.MDefaultChase)]
public class MDefaultChaseState : EnemyChaseBaseState
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

		if (curTime > unit.beforeChaseDelay)
		{
			if (distance < unit.attackRange)
			{
				unit.rigid.velocity = Vector3.zero;
				unit.navMesh.enabled = false;
				unit.ChangeState(EnemyState.MDefaultAttack);
			}
			else if (distance > unit.attackRange && distance < targetDistance)
				unit.navMesh.SetDestination(unit.target.transform.position);
			else if (distance > targetDistance)
				unit.ChangeState(EnemyState.Default);
		}
	}
}
