using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyState.Idle)]
public class EnemyIdleState : StateBase
{
	private float idleSetTime = 3f;

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		unit.DelayChangeState(curTime, idleSetTime, unit, EnemyState.Default);
	}

	public override void End(EnemyController unit)
	{
		curTime = 0;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			if(unit.target == null)
				unit.target = other.GetComponent<UnitBase>();
			unit.ChangeState(unit.UnitChaseState());
		}
	}
}
