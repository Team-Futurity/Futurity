using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[FSMState((int)EnemyController.EnemyState.RDefaultAttack)]
public class RDefaultAttackState : UnitState<EnemyController>
{
	private float curTime = 0f;
	private Vector3 projectilePos = Vector3.zero;
	public override void Begin(EnemyController unit)
	{
		FDebug.Log("Attack Begin");

		unit.rangedProjectile.SetActive(true);
	}

	public override void Update(EnemyController unit)
	{
		float distance = Vector3.Distance(unit.transform.position, unit.rangedProjectile.transform.position);
		curTime += Time.deltaTime;
		
		if (distance > unit.projectileDistance)
			unit.rangedProjectile.SetActive(false);

		unit.DelayChangeState(curTime, unit.attackSetTime, unit, EnemyController.EnemyState.RDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("Attack End");
		unit.rangedProjectile.transform.position = projectilePos;
		curTime = 0f;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player"))
		{
			FDebug.Log("Attack success");
		}
	}
}
