using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.Idle)]
public class EnemyIdleState : UnitState<EnemyController>
{
	float curTime;

	public override void Begin(EnemyController unit)
	{
		curTime = 0;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		unit.DelayChangeState(curTime, unit.idleSetTime, unit, EnemyController.EnemyState.Default);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player") && !unit.isChasing)
		{
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeChaseState(unit);
		}
	}
}
