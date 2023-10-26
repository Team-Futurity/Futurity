using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[FSMState((int)EnemyState.EliteMeleeAttack)]
public class EliteMeleeAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();
	private float radius = 3.27f;

	public EliteMeleeAttackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Caster;
		atk1.index = 0;
		atk1.position = new Vector3(0, 1.5f, 0);
	}


	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		unit.currentEffectData = atk1;
		unit.atkCollider.radius = radius;
		unit.rigid.velocity = Vector3.zero;
		unit.enemyData.status.GetStatus(StatusType.ATTACK_POINT).SetValue(38);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, attackChangeDelay, unit, EnemyState.EliteDefaultChase);
	}
}
