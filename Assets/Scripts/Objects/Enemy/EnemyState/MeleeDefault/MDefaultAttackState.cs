using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultAttack)]
public class MDefaultAttackState : UnitState<EnemyController>
{
	private float curTime = .0f;
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack begin");
		unit.animator.SetTrigger(unit.atkAnimParam);
		unit.atkCollider.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.MDefaultAttack2nd);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack End");
		unit.atkCollider.enabled = false;
		curTime = 0f;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			//FDebug.Log("MDefault Attack Trigger");
			unit.enemyData.Attack(unit.target);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
