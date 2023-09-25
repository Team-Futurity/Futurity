using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBaseState : UnitState<EnemyController>
{
	protected float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.atkAnimParam);
		unit.enemyData.EnableAttackTime();
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, unit.UnitChaseState());
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		curTime = 0f;
		unit.enemyData.DisableAttackTime();
		/*unit.effectManager.HitEffectDeActive(unit.hitEffect.indexNum);
		foreach(var i in unit.effects)
		{
			if (unit.effectManager.copySpecific[i.indexNum] != null)
				unit.effectManager.SpecificEffectDeActive(i.indexNum);
		}*/
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{
		
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
			unit.enemyData.Attack(unit.target);
	}
}
