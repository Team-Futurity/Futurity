using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.RDefaultDelay)]
public class RDefaultDelayState : StateBase
{
	private float attackAfterDelay = 2.0f;

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, attackAfterDelay, unit, EnemyState.RDefaultChase);
	}
}
