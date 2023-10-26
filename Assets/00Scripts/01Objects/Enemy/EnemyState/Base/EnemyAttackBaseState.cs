using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBaseState : StateBase
{

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.atkAnimParam);
		unit.enemyData.EnableAttackTime();
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackingDelay, unit, unit.UnitChaseState());
	}

	public override void End(EnemyController unit)
	{
		curTime = 0f;
		unit.enemyData.DisableAttackTime();
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			DamageInfo info = new DamageInfo(unit.enemyData, unit.target, 1);
			unit.enemyData.Attack(info);
		}
	}
}
