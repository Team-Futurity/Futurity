using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultAttack2nd)]
public class MDefaultAttack2ndState : UnitState<EnemyController>
{
	private float curTime;
	public override void Begin(EnemyController unit)
	{
		FDebug.Log("MDefault Attack2nd begin");
		unit.animator.SetTrigger(unit.atkAnimParam);
		curTime = 0f;
		unit.atkRange.enabled = false;
		unit.atkCollider.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackSetTime, unit, EnemyController.EnemyState.MDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("MDefault Attack2nd End");
		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			FDebug.Log("MDefault Attack2nd Trigger");
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
