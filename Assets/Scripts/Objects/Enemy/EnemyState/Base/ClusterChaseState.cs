using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.ClusterChase)]
public class ClusterChaseState : EnemyChaseBaseState
{
	private float attackDistance = .0f;
	private float clusterDistance = .0f;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("cluster chase begin");
		base.Begin(unit);

	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		attackDistance = Vector3.Distance(unit.clusteringManager.clusters[unit.clusterNum].enemys[0].transform.position, unit.target.transform.position);
		clusterDistance = Vector3.Distance(unit.clusterTarget.transform.position, unit.transform.position);

		if (distance < unit.attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
		}

		else if (attackDistance > unit.attackRange)
		{
			if (clusterDistance < unit.clusterDistance)
				unit.ChangeState(EnemyController.EnemyState.ClusterSlow);
			else
				unit.navMesh.SetDestination(unit.clusterTarget.transform.position);
		}

		else if (attackDistance < unit.attackRange)
			unit.navMesh.SetDestination(unit.target.transform.position);
	}

	public override void End(EnemyController unit)
	{
		base.End(unit);

	}
}
