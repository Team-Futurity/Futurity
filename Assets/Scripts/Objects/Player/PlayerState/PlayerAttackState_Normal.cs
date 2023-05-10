using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.NormalAttack)]
public class PlayerAttackState_Normal : PlayerAttackState
{
	public override void Begin(PlayerController pc)
	{
		base.Begin(pc);

		AudioManager.instance.PlayOneShot(attackNode.attackSound, pc.transform.position);

		//юс╫ц
		pc.glove.SetActive(true);
		pc.rigid.velocity = Vector3.zero;
		pc.rigid.velocity = pc.transform.forward * attackNode.moveDistance;
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
