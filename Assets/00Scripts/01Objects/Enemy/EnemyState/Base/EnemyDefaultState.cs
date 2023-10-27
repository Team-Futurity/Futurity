using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyState.Default)]
public class EnemyDefaultState : StateBase
{
	private float randMoveFloat = .0f;
	private float movePercentage = 5f;

	public override void Begin(EnemyController unit)
	{
		randMoveFloat = Random.Range(0, 10);
	}

	public override void Update(EnemyController unit)
	{
		if (randMoveFloat < movePercentage)
		{
			unit.ChangeState(EnemyState.MoveIdle);
		}
		else
		{
			unit.ChangeState(EnemyState.Idle);
		}
	}
	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeState(unit.UnitChaseState());
		}
	}
}
