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
	}

	public override void Update(EnemyController unit)
	{
		if (unit.randMoveFloat < unit.movePercentage)
		{
			if (!unit.IsCurrentState(EnemyController.EnemyState.MoveIdle))
			{
				unit.ChangeState(EnemyController.EnemyState.MoveIdle);
			}
		}
		else
		{
			if (!unit.IsCurrentState(EnemyController.EnemyState.Idle))
			{
				unit.ChangeState(EnemyController.EnemyState.Idle);
			}
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player") && !unit.isChasing)
		{
			unit.target = other.GetComponent<UnitBase>();
			if (!unit.IsCurrentState(EnemyController.EnemyState.Chase))
			{
				unit.ChangeState(EnemyController.EnemyState.Chase);
			}
		}
	}
}
