using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.ClusterChase)]
public class ClusterChaseState : EnemyChaseBaseState
{
	private float attackDistance;
	private float clusterDistance;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("cluster begin");
		base.Begin(unit);

		unit.clusterTarget = unit.clusteringManager.clusters[unit.clusterNum].enemys[unit.individualNum - 1];
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);
		attackDistance = Vector3.Distance(unit.clusteringManager.clusters[unit.clusterNum].enemys[0].transform.position, unit.target.transform.position);
		clusterDistance = Vector3.Distance(unit.clusterTarget.transform.position, unit.transform.position);

		if (attackDistance < unit.attackRange)
		{
			if (distance < unit.attackRange)
			{
				unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
			}
			else
			{
				unit.navMesh.SetDestination(unit.target.transform.position);
			}
		}
		else if(attackDistance > unit.attackRange)
		{
			if (clusterDistance < unit.clusterDistance)
				unit.ChangeState(EnemyController.EnemyState.ClusterSlow);
			else
				unit.navMesh.SetDestination(unit.clusterTarget.transform.position);
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("cluster end");

		base.End(unit);
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}
}
