using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.Death)]
public class B_DeathState : UnitState<BossController>
{
	public override void Begin(BossController unit)
	{
		unit.animator.SetTrigger(unit.deathAnim);
		unit.isDead = true;
		
		TimelineManager.Instance.EnableCutScene(ECutSceneType.BOSS_DEATH);
	}

	public override void End(BossController unit)
	{
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
	}
}
