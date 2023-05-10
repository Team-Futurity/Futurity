using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Default)]
public class EnemyDefaultState : UnitState<EnemyController>
{

	public override void Begin(EnemyController unit)
	{
		unit.randMoveFloat = Random.Range(0, 10);
		FDebug.Log("EnemyDefault");


		//임시
		unit.isDefault= true;
	}

	public override void Update(EnemyController unit)
	{
		//if(unit.randMoveFloat < unit.movePercentage)
		//{
		//	if (!unit.IsCurrentState(EnemyController.EnemyState.MoveIdle))
		//	{
		//		unit.ChangeState(EnemyController.EnemyState.MoveIdle);
		//	}
		//}
		//else
		//{
		//	if (!unit.IsCurrentState(EnemyController.EnemyState.Idle))
		//	{
		//		unit.ChangeState(EnemyController.EnemyState.Idle);
		//	}
		//}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("EnemyDefaultEnd");


		//임시
		unit.isDefault = false;
		unit.stayCurTime = 0f;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player") && !unit.isChasing)
		{
			FDebug.Log("Default Chasing");
			unit.target = other.GetComponent<UnitBase>();
			if (!unit.IsCurrentState(EnemyController.EnemyState.Chase))
			{
				unit.ChangeState(EnemyController.EnemyState.Chase);
			}
		}
	}
}
