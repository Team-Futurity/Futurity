using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[FSMState((int)EnemyController.EnemyState.EliteMeleeAttack)]
public class EliteMeleeAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();

	public EliteMeleeAttackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Caster;
		atk1.index = 0;
	}


	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		atk1.position = new Vector3(0, 1.5f, 0);
		atk1.parent = unit.gameObject;
		unit.currentEffectData = atk1;
		unit.rigid.velocity = Vector3.zero;
		unit.navMesh.enabled = true;
		unit.atkCollider.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.EliteDefaultChase);
	}

	public override void End(EnemyController unit)
	{

		base.End(unit);
		unit.atkCollider.enabled = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}
}
