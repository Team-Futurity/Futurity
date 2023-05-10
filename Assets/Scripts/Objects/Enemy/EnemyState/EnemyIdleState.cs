using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Idle)]
public class EnemyIdleState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		FDebug.Log("EnemyIdle");
	}

	public override void Update(EnemyController unit)
	{
		//unit.stayCurTime += Time.deltaTime;

		//if(unit.stayCurTime > unit.stayMaxTime)
		//{
		//	if (!unit.IsCurrentState(EnemyController.EnemyState.Default))
		//	{
		//		unit.ChangeState(EnemyController.EnemyState.Default);
		//	}
		//}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.stayCurTime = 0f;
		FDebug.Log("EnemyIdleEnd");

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player") && !unit.isChasing)
		{
			FDebug.Log("Idle Chasing");
			unit.target = other.GetComponent<UnitBase>();
			if (!unit.IsCurrentState(EnemyController.EnemyState.Chase))
			{
				unit.ChangeState(EnemyController.EnemyState.Chase);
			}
		}
	}
}
