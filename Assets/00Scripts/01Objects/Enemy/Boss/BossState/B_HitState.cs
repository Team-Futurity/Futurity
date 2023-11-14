using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

[FSMState((int)BossState.Hit)]
public class B_HitState : UnitState<BossController>
{
	private float curHP = 0;
	private bool isColorChanged = false;
	private float curTime;
	private Color defaultColor = new Color(1, 1, 1, 0f);

	private bool is25PerEventDone = false;

	public override void Begin(BossController unit)
	{
		//unit.curState = BossState.Hit;
		curTime = 0;
		curHP = unit.bossData.status.GetStatus(StatusType.CURRENT_HP).GetValue();
		if (unit.curState == BossState.Chase || unit.curState == BossState.Idle)
		{
			unit.nextState = BossState.Idle;
			unit.animator.SetTrigger(unit.hitAnim);
		}
		unit.copyUMat.SetColor("_BaseColor", unit.damagedColor);
	}
	public override void Update(BossController unit)
	{
		curTime += Time.deltaTime;

		if (!isColorChanged)
			if (curTime > unit.hitColorChangeTime)
			{
				unit.copyUMat.SetColor("_BaseColor", defaultColor);
				isColorChanged = true;
			}

		if (curTime > unit.hitMaxTime)
			unit.RemoveSubState();


		//unit.DelayChangeState(curTime, unit.hitMaxTime, BossState.Idle);

		//Phase event
		if (!unit.isPhase2EventDone && curHP <= unit.bossData.status.GetStatus(StatusType.MAX_HP).GetValue() * unit.phaseDataSO.GetHPPercentage(Phase.Phase2))
		{
			unit.curPhase = Phase.Phase2;
			unit.ChangeState(BossState.Phase2Event);
		}
		if (!is25PerEventDone && curHP <= unit.bossData.status.GetStatus(StatusType.MAX_HP).GetValue() * 0.25f)
		{
			unit.ChangeState(BossState.T5_EnemySpawn);
		}
		//Death event
		if (unit.bossData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
		{
			unit.ChangeState(BossState.Death);
		}
	}

	public override void End(BossController unit)
	{
		curHP = unit.bossData.status.GetStatus(StatusType.CURRENT_HP).GetValue();

		if (!is25PerEventDone && curHP <= unit.bossData.status.GetStatus(StatusType.MAX_HP).GetValue() * 0.25f)
		{
			is25PerEventDone = true;
		}

		unit.rigid.velocity = Vector3.zero;
		isColorChanged = false;
		unit.copyUMat.SetColor("_BaseColor", defaultColor);
	}

	public override void FixedUpdate(BossController unit)
	{
	}

	public override void OnCollisionEnter(BossController unit, Collision collision)
	{
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
	}
}
