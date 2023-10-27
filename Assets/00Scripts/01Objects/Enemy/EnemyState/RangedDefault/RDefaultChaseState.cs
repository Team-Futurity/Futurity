using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[FSMState((int)EnemyState.RDefaultChase)]
public class RDefaultChaseState : EnemyChaseBaseState
{
	private float attackBeforeDelay = 3f;

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		unit.transform.LookAt(unit.target.transform.position);
		 if (distance < unit.attackRange)
		{
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.DelayChangeState(curTime, attackBeforeDelay, unit, EnemyState.RDefaultAttack);
		}
		else if (distance > unit.attackRange)
		{
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}
}
