using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultAttack)]
public class MiniDefaultAttackState : UnitState<EnemyController>
{
	private float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack begin");
		unit.animator.SetTrigger(unit.atkAnimParam);
		unit.atkCollider.enabled = true;
		unit.rigid.AddForce(unit.transform.forward * 10.0f, ForceMode.Acceleration);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackDelayTime, unit, EnemyController.EnemyState.MiniDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack End");
		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			//FDebug.Log("MiniDefault Attack Trigger");
			unit.enemyData.Attack(unit.target);
			unit.ChangeState(EnemyController.EnemyState.MiniDefaultKnockback);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
