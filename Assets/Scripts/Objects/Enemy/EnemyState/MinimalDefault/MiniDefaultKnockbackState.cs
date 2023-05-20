using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultAttack)]
public class MiniDefaultKnockbackState : UnitState<EnemyController>
{
	private float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		unit.rigid.AddForce(-unit.transform.forward * 10.0f, ForceMode.Impulse);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, 2.0f, unit, EnemyController.EnemyState.MiniDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		curTime = 0f;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
