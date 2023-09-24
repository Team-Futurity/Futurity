using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultChase)]
public class MDefaultChaseState : EnemyChaseBaseState
{
	private float attackDistance = .0f;
	private float clusterDistance = .0f;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Chase begin");

		base.Begin(unit);


		if (unit.isClustering && unit.individualNum > 0)
			unit.ChangeState(EnemyController.EnemyState.ClusterChase);
	}
	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		if (distance < unit.attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
		}

		else
			unit.navMesh.SetDestination(unit.target.transform.position);
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MDefault Chase end");

		base.End(unit);
	}
}
