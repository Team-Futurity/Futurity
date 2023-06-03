using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.ClusterSlow)]
public class ClusterSlowState : UnitState<EnemyController>
{
	private float distance = .0f;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("Cluster slow begin");

		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() / 2;
	}

	public override void Update(EnemyController unit)
	{
		distance = Vector3.Distance(unit.clusterTarget.transform.position, unit.transform.position);

		if (distance > unit.clusterDistance)
			unit.ChangeState(EnemyController.EnemyState.ClusterChase);

		else
		{
			unit.navMesh.SetDestination(unit.clusterTarget.transform.position);
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("Cluster slow end");

		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue();
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}
}
