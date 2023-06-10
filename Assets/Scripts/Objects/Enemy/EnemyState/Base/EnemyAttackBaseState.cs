using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBaseState : UnitState<EnemyController>
{
	protected float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.atkAnimParam);
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
		if (EnemyEffectManager.Instance.copyHit[unit.hitEffect.indexNum] != null)
			EnemyEffectManager.Instance.HitEffectDeActive(unit.hitEffect.indexNum);
		foreach(var i in unit.effects)
		{
			if (EnemyEffectManager.Instance.copySpecific[i.indexNum] != null)
				EnemyEffectManager.Instance.SpecificEffectDeActive(i.indexNum);
		}
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
