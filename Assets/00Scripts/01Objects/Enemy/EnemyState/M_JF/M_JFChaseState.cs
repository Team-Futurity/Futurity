using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.M_JFChase)]
public class M_JFChaseState : EnemyChaseBaseState
{
	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		if (curTime > unit.beforeChaseDelay)
		{
			if (distance < unit.attackRange)
			{
				unit.rigid.velocity = Vector3.zero;
				unit.navMesh.enabled = false;
				unit.ChangeState(EnemyState.M_JFAttack);
			}
			else if (distance > unit.attackRange && distance < targetDistance)
				unit.navMesh.SetDestination(unit.target.transform.position);
			else if (distance > targetDistance)
				unit.ChangeState(EnemyState.Default);
		}
	}
}
