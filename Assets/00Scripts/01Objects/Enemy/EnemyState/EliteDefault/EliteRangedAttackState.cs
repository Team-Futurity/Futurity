using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[FSMState((int)EnemyController.EnemyState.EliteRangedAttack)]
public class EliteRangedAttackState : EnemyAttackBaseState
{
	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.ragnedAnimParam);

		unit.navMesh.enabled = true;
		unit.rigid.velocity = Vector3.zero;
		/*unit.atkCollider.transform.position = unit.target.transform.position;
		unit.effects[1].effectTransform.position = unit.target.transform.position;*/
		//unit.atkCollider.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.EliteDefaultChase);
	}

	public override void End(EnemyController unit)
	{
		unit.atkCollider.transform.position = unit.transform.position;
		//unit.effects[1].effectTransform.position = unit.transform.position;
		unit.atkCollider.enabled = false;
		base.End(unit);

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}
}
