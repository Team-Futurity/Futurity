using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.ClusterSlow)]
public class ClusterSlowState : EnemyChaseBaseState
{
	private float clusterDistance = .0f;
	private float attackDistance = .0f;
	private bool isEncircle = false;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("Cluster slow begin");

		base.Begin(unit);
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() / 2;
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		clusterDistance = Vector3.Distance(unit.clusterTarget.transform.position, unit.transform.position);
		attackDistance = Vector3.Distance(unit.clusterTarget.transform.position, unit.target.transform.position);

		if (distance < unit.attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
		}

		if (attackDistance < unit.attackRange)
			unit.ChangeState(EnemyController.EnemyState.MDefaultChase);
		else
		{
			if (clusterDistance < unit.clusterDistance)
			{
				unit.navMesh.SetDestination(unit.clusterTarget.transform.position);
			}
			else
				unit.ChangeState(EnemyController.EnemyState.MDefaultChase);
		}
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("Cluster slow end");

		base.End(unit);
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue();
	}
}
