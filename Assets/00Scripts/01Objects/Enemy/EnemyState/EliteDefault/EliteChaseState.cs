using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.EliteDefaultChase)]
public class EliteChaseState : EnemyChaseBaseState
{
	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		unit.transform.LookAt(unit.target.transform.position);


		if (distance < attackRange * 0.5f)
		{
			unit.ChangeState(EnemyState.EliteMeleeAttack);
		}
		else if (distance < attackRange)
		{
			unit.ChangeState(EnemyState.EliteRangedAttack);
		}
		else if (distance > attackRange)
		{
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}
}
