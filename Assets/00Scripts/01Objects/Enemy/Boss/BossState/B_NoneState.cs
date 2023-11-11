using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[FSMState((int)BossState.Phase2Event)]
public class B_NoneState : UnitState<BossController>
{
	public override void Begin(BossController unit)
	{
		unit.curState = BossState.Phase2Event;
		unit.isActive = false;
		unit.isPhase2EventDone = true;
		unit.isInPhase2Event = true;
		unit.nextState = BossState.T5_EnemySpawn;
		
		TimelineManager.Instance.EnableCutScene(ECutSceneType.BOSS_ENDPHASE);
	}

	public override void End(BossController unit)
	{
		unit.previousState = BossState.Phase2Event;
		unit.isInPhase2Event = false;
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

	public override void Update(BossController unit)
	{
		if (unit.isActive)
			unit.ChangeState(unit.nextState);
	}
}
