using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.RDefaultAttack)]
public class RDefaultAttackState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		FDebug.Log("Attack Begin");


	}

	public override void End(EnemyController unit)
	{

	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void Update(EnemyController unit)
	{

	}
}
