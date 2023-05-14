using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Hitted)]
public class EnemyHittedState : UnitState<EnemyController>
{
	private float curTime;
	public override void Begin(EnemyController unit)
	{
		FDebug.Log("Hit Begin");
		curTime = 0;
		unit.animator.SetTrigger(unit.hitAnimParam);
		unit.eMaterial.color = Color.red;
	}

	public override void Update(EnemyController unit)
	{
		//여기서 death 연산 까지
		curTime += Time.deltaTime;

		unit.DelayChangeState(curTime, unit.hitMaxTime, unit, EnemyController.EnemyState.MDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{
		
	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("Hit End");
		unit.eMaterial.color = unit.defaultColor;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
