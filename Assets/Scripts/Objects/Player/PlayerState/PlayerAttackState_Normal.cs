using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.NormalAttack)]
public class PlayerAttackState_Normal : PlayerAttackState
{
	public PlayerAttackState_Normal() : base("ComboTrigger", "Combo") { }

	public override void Begin(PlayerController pc)
	{
		base.Begin(pc);

		AudioManager.instance.PlayOneShot(attackNode.attackSound, pc.transform.position);
	}

	public override void End(PlayerController pc)
	{
		base.End(pc);
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}

	public override void Update(PlayerController pc)
	{
		base.Update(pc);
	}
}
