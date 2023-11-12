using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBaseState : StateBase
{
	protected bool isAttack = false;


	public override void Begin(EnemyController unit)
	{
		unit.enemyData.EnableAttackTime();
		unit.rigid.velocity = Vector3.zero;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		if(!isAttack)
		{
			AttackAnim(unit, curTime, unit.attackBeforeDelay);
		}
		else
			unit.DelayChangeState(curTime, unit.attackingDelay, unit, unit.UnitChaseState());
	}

	public override void End(EnemyController unit)
	{
		curTime = 0f;
		isAttack = false;
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


	protected virtual void AttackAnim(EnemyController unit, float curTime, float maxTime)
	{
		if (curTime > maxTime)
		{
			if(unit.ThisEnemyType == EnemyType.MeleeDefault)
				AudioManager.Instance.PlayOneShot(unit.attackSound1, unit.transform.position);
			unit.animator.SetTrigger(unit.atkAnimParam);
			curTime = 0f;
			isAttack = true;
		}
	}
}
