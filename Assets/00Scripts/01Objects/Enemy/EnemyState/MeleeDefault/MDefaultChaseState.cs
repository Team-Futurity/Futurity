using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[FSMState((int)EnemyState.MDefaultChase)]
public class MDefaultChaseState : EnemyChaseBaseState
{
	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		if (distance < attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.ChangeState(EnemyState.MDefaultAttack);
		}

		else
			unit.navMesh.SetDestination(unit.target.transform.position);
	}
}
