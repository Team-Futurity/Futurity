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
		FDebug.Log("Default Begin");
		unit.randMoveFloat = Random.Range(0, 10);
	}

	public override void Update(EnemyController unit)
	{
		if (unit.randMoveFloat < unit.movePercentage)
		{
			unit.ChangeState(EnemyController.EnemyState.MoveIdle);
		}
		else
		{
			unit.ChangeState(EnemyController.EnemyState.Idle);
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("Default End");
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag) && !unit.isChasing)
		{
			FDebug.Log("Default Trigger");
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeChaseState(unit);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
