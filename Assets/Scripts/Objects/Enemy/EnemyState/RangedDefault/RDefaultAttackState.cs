using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

[FSMState((int)EnemyController.EnemyState.RDefaultAttack)]
public class RDefaultAttackState : UnitState<EnemyController>
{
	private float curTime;
	public override void Begin(EnemyController unit)
	{
		FDebug.Log("Attack Begin");

		unit.rangedProjectile.transform.position = new Vector3(0, 0, 0);
		unit.rangedProjectile.SetActive(true);
	}

	public override void Update(EnemyController unit)
	{
		float distance = Vector3.Distance(unit.transform.position, unit.rangedProjectile.transform.position);
		if (distance < unit.rangedAttackDistance)
			unit.rangedProjectile.SetActive(false);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("Attack End");
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player"))
		{
			FDebug.Log("Attack success");
			unit.rangedProjectile.SetActive(false);
			unit.ChangeState(EnemyController.EnemyState.RDefaultChase);
		}
	}
}
