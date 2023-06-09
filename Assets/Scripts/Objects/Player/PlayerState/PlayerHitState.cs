using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.Hit)]
public class PlayerHitState : UnitState<PlayerController>
{
	private readonly string HitTriggerAnim = "HitTrigger";

	public override void Begin(PlayerController pc)
	{
		pc.animator.SetBool(pc.IsAttackingAnimKey, false);
		pc.animator.SetTrigger(HitTriggerAnim);
		pc.rmController.SetRootMotion("Hit");
		pc.specialIsReleased = false;
		pc.curNode = pc.comboTree.top;

		Camera.main.gameObject.GetComponent<PostProcessController>().SetVignette(0.5f);

		pc.glove.SetActive(false);

		pc.ChangeState(PlayerState.Idle);
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
		pc.hitCoolTimeIsEnd = false;
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
