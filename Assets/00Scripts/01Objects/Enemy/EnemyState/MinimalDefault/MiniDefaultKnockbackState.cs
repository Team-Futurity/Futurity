using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultKnockback)]
public class MiniDefaultKnockbackState : UnitState<EnemyController>
{
	private float curTime = .0f;

	private EffectActiveData atk1 = new EffectActiveData();

	public MiniDefaultKnockbackState()
	{
		atk1.activationTime = EffectActivationTime.InstanceAttack;
		atk1.target = EffectTarget.Target;
		atk1.index = 0;
	}

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.atkAnimParam);
		atk1.position = unit.target.transform.position;
		unit.currentEffectData = atk1;
		unit.rigid.AddForce(-unit.transform.forward * unit.powerReference2, ForceMode.Impulse);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, 0.5f, unit, EnemyController.EnemyState.MiniDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.rigid.velocity = Vector3.zero;
		curTime = 0f;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
