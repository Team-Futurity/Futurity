using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultKnockback)]
public class MiniDefaultKnockbackState : UnitState<EnemyController>
{
	private float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetTrigger(unit.atkAnimParam);
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
