using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.Hit)]
public class PlayerHitState : UnitState<PlayerController>
{
	private readonly string HitTriggerAnim = "HitTrigger";
	private float currentTime;

	public override void Begin(PlayerController pc)
	{
		pc.animator.SetBool(pc.IsAttackingAnimKey, false);
		pc.animator.SetTrigger(HitTriggerAnim);
		pc.rmController.SetRootMotion("Hit");
		pc.specialIsReleased = false;
		pc.curNode = pc.commandTree.Top;

		currentTime = 0;

		pc.SetGauntlet(false);

		if (pc.playerData.status.GetStatus(StatusType.CURRENT_HP).GetValue() > 0)
		{
			pc.camera?.StartHitEffectVignette();
		}
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime >= pc.hitCoolTime)
		{
			if (pc.playerData.status.GetStatus(StatusType.CURRENT_HP).GetValue() <= 0)
			{
				pc.ChangeState(PlayerState.Death);
			}
			else
			{
				pc.ChangeState(PlayerState.Idle);
			}
		}
		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		//base.End(pc);
		pc.rigid.velocity = Vector3.zero;
		//pc.hitCoolTimeIsEnd = false;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		throw new System.NotImplementedException();
	}
}
