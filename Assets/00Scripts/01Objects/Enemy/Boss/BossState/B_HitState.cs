using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

[FSMState((int)BossController.BossState.Hit)]
public class B_HitState : UnitState<BossController>
{
	private float curHP = 0;

	private bool isColorChanged = false;
	private float curTime;
	private Color defaultColor = new Color(1, 1, 1, 0f);

	public override void Begin(BossController unit)
	{
		unit.curState = BossController.BossState.Hit;

		curTime = 0;
		unit.animator.SetTrigger(unit.hitAnim);
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


		unit.DelayChangeState(curTime, unit.hitMaxTime, );

		//Death event
		if (unit.bossData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
		{
			unit.ChangeState(EnemyState.Death);
		}
	}

	public override void End(BossController unit)
	{
		curHP = unit.bossData.status.GetStatus(StatusType.CURRENT_HP).GetValue();
		if (curHP < unit.bossData.status.GetStatus(StatusType.MAX_HP).GetValue() * unit.phaseDataSO.GetPhasePercentage(Phase.Phase4))
			SetPhaseData(unit, Phase.Phase4);
		else if (curHP < unit.bossData.status.GetStatus(StatusType.MAX_HP).GetValue() * unit.phaseDataSO.GetPhasePercentage(Phase.Phase3))
			SetPhaseData(unit, Phase.Phase3);
		else if (curHP < unit.bossData.status.GetStatus(StatusType.MAX_HP).GetValue() * unit.phaseDataSO.GetPhasePercentage(Phase.Phase2))
			SetPhaseData(unit, Phase.Phase2);
		else if (curHP < unit.bossData.status.GetStatus(StatusType.MAX_HP).GetValue() * unit.phaseDataSO.GetPhasePercentage(Phase.Phase1))
			SetPhaseData(unit, Phase.Phase1);

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

	private void SetPhaseData(BossController unit, Phase phase)
	{
		unit.curPhase = phase;
		unit.type467MaxTime = unit.phaseDataSO.GetType467TImerValue(phase);
		unit.type5MaxTime = unit.phaseDataSO.GetType5TImerValue(phase);
	}
}
