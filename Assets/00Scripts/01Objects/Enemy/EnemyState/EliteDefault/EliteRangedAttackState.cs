using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[FSMState((int)EnemyController.EnemyState.EliteRangedAttack)]
public class EliteRangedAttackState : EnemyAttackBaseState
{
	private EffectActiveData atk1 = new EffectActiveData();

	public EliteRangedAttackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Target;
		atk1.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.ragnedAnimParam);
		
		atk1.position = new Vector3(-0.1f, -0.3f, -0.015f);
		atk1.parent = unit.test.gameObject;
		unit.currentEffectData = atk1;
		unit.navMesh.enabled = true;
		unit.rigid.velocity = Vector3.zero;
		
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
		//unit.atkCollider.enabled = false;
		base.End(unit);

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}
}
