using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultAttack2nd)]
public class MDefaultAttack2ndState : EnemyAttackBaseState
{
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack2nd begin");
		unit.atkCollider.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MDefault Attack2nd End");

		base.End(unit);
		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		//FDebug.Log("MDefault Attack2nd Trigger");
		base.OnTriggerEnter(unit, other);
	}
}
