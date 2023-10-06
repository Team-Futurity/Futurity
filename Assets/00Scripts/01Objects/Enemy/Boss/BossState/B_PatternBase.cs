using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_PatternBase : UnitState<BossController>
{
	public float curTime = 0f;


	public override void Begin(BossController unit)
	{
		unit.bossData.EnableAttackTime();
	}
	public override void Update(BossController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, unit.activeDataSO.GetActivateDelayValue(unit.curState), BossController.BossState.Idle);
	}

	public override void End(BossController unit)
	{
		unit.skillAfterDelay = unit.activeDataSO.GetAfterDelayValue(unit.curPhase, unit.curState);
		unit.bossData.DisableAttackTime();
	}

	public override void FixedUpdate(BossController unit)
	{
	}

	public override void OnCollisionEnter(BossController unit, Collision collision)
	{
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
		if (other.CompareTag("Player"))
			unit.bossData.Attack(unit.target);
	}
}
