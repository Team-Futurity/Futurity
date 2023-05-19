using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.Hit)]
public class PlayerHitState : UnitState<PlayerController>
{
	public override void Begin(PlayerController pc)
	{
		pc.animator.SetBool(pc.IsAttackingAnimKey, false);
		pc.specialIsReleased = false;
		pc.curNode = pc.comboTree.top;

		Camera.main.gameObject.GetComponent<PostProcessController>().SetVignette(0.5f);

		pc.glove.SetActive(false);

		pc.ChangeState(PlayerController.PlayerState.Idle);
	}

	public override void Update(PlayerController pc)
	{

	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		//base.End(pc);
		pc.rigid.velocity = Vector3.zero;
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
