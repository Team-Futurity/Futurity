using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.Death)]
public class PlayerDeathState : UnitState<PlayerController>
{
	private readonly string DeathAnimKey = "IsDead";
	private readonly string DeathAnimTriggerKey = "DeadTrigger";

	public override void Begin(PlayerController pc)
	{
		TimelineManager.Instance.EnableCutScene(ECutSceneType.PLAYER_DEATH);
		UIManager.Instance.OpenWindowAllClose();
		
		pc.animator.SetTrigger(DeathAnimTriggerKey);
		pc.animator.SetBool(DeathAnimKey, true);
		pc.rmController.SetRootMotion("Death");
	}

	public override void Update(PlayerController pc)
	{

	}

	public override void FixedUpdate(PlayerController unit)
	{
		if (!unit.playerData.isKnockbaking)
		{
			unit.rigid.velocity = Vector3.zero;
		}
	}

	public override void End(PlayerController pc)
	{
		pc.animator.SetBool(DeathAnimKey, false);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
