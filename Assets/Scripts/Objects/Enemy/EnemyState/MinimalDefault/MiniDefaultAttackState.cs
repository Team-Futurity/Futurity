using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultAttack)]
public class MiniDefaultAttackState : EnemyAttackBaseState
{

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack begin");
		base.Begin(unit);
		unit.atkCollider.enabled = true;
		unit.rigid.AddForce(unit.transform.forward * unit.powerReference1, ForceMode.Impulse);
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);
	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Attack End");
		unit.atkCollider.enabled = false;
		base.End(unit);
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
}
