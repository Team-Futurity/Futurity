using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Death)]
public class EnemyDeathState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{

	}

	public override void Update(EnemyController unit)
	{

	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{
		throw new System.NotImplementedException();
	}
}
