using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultClusterChase)]
public class MDefaultClusterState : EnemyChaseBaseState
{
	private EnemyController moveTarget;
	private float clusterDistance;

	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		FDebug.Log("cluster begin");
		moveTarget = unit.clusteringManager.clusters[unit.clusterNum].enemys[unit.individualNum - 1];
	}
	public override void Update(EnemyController unit)
	{
		base.Update(unit);
		clusterDistance = Vector3.Distance(unit.clusteringManager.clusters[unit.clusterNum].enemys[0].transform.position, unit.target.transform.position);

		if(clusterDistance < unit.attackRange)
		{
			if (distance < unit.attackRange)
			{
				FDebug.Log("1");
				unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
			}
			else
			{
				FDebug.Log("1");
				unit.navMesh.SetDestination(unit.target.transform.position);
			}
		}
		else if(clusterDistance > unit.attackRange)
		{
			unit.navMesh.SetDestination(moveTarget.transform.position);
		}
	}
	public override void FixedUpdate(EnemyController unit)
	{

	}
	public override void End(EnemyController unit)
	{
		base.End(unit);
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}
}
