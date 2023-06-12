using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Idle)]
public class EnemyIdleState : UnitState<EnemyController>
{
	float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("IdleBegin");

		//unit.ChangeState(EnemyController.EnemyState.Spawn);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		unit.DelayChangeState(curTime, unit.idleSetTime, unit, EnemyController.EnemyState.Default);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("IdleEnd");
		curTime = 0;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag) /*&& !unit.isChasing*/)
		{
			//FDebug.Log("Idle Trigger");
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeState(unit.UnitChaseState());

			if (unit.isClusteringObj)
				EnemyManager.Instance.EnemyClustering(unit);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
