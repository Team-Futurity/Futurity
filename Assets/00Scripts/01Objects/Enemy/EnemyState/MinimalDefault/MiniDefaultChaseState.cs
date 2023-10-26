using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.MiniDefaultChase)]
public class MiniDefaultChaseState : EnemyChaseBaseState
{
	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		unit.transform.LookAt(unit.target.transform.position);

		if (distance < attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.ChangeState(EnemyState.MiniDefaultDelay);
		}
		else if (distance > attackRange)
		{
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}
}
