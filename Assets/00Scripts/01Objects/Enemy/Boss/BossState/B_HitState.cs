using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.Hit)]
public class B_HitState : UnitState<BossController>
{
	float curHP = 0;

	public override void Begin(BossController unit)
	{
		unit.curState = BossController.BossState.Hit;
	}
	public override void Update(BossController unit)
	{

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
