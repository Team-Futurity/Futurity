using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_PatternBase : BossStateBase
{
	protected bool isAttackDone = false;
	protected Vector3 targetPos = new Vector3();

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.bossData.EnableAttackTime();
		unit.activeDataSO.SetCurAttackData(unit);
		unit.nextState = BossState.Idle;
	}

	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		isAttackDone = false;
		unit.bossData.DisableAttackTime();
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
	}
}
