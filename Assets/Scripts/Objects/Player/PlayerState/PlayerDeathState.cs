using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.Death)]
public class PlayerDeathState : UnitState<PlayerController>
{
	private readonly string DeathAnimKey = "IsDead";

	public override void Begin(PlayerController pc)
	{
		TimelineManager.Instance.EnableCutScene(TimelineManager.ECutScene.PLYAERDEATHCUTSCENE);
		
		pc.animator.SetBool(DeathAnimKey, true);
		pc.rmController.SetRootMotion("Death");
	}

	public override void Update(PlayerController pc)
	{

	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		pc.animator.SetBool(DeathAnimKey, false);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		throw new System.NotImplementedException();
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		throw new System.NotImplementedException();
	}
}
