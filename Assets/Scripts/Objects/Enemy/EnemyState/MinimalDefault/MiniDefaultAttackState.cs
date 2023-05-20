using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultAttack)]
public class MiniDefaultAttackState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack begin");
	}

	public override void Update(EnemyController unit)
	{

	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack End");
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			//FDebug.Log("MiniDefault Attack Trigger");
			unit.enemyData.Attack(unit.target);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
