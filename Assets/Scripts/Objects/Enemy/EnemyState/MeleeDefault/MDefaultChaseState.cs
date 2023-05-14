using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultChase)]
public class EnemyChaseState : UnitState<EnemyController>
{
	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.atkRange.enabled = true;
		unit.isChasing = true;
	}
	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;
		unit.transform.LookAt(unit.target.transform.position);
		unit.transform.position += unit.transform.forward * unit.enemyData.Speed * Time.deltaTime;
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.isChasing = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			unit.rigid.velocity = Vector3.zero;
			unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
		}
	}
}
