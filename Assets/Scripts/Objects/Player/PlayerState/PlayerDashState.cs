using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.Dash)]
public class PlayerDashState : UnitState<PlayerController>
{
	// anim keys
	private readonly string DashTriggerAnimKey = "Dash";

	// etc
	private float currentTime;
	private const float dashTime = 0.2f;
	public override void Begin(PlayerController pc)
	{
		//base.Begin(pc);
		pc.animator.SetTrigger(DashTriggerAnimKey);
		currentTime = 0;
		pc.dashEffect.enabled = true;
		pc.rigid.velocity = pc.transform.forward * pc.playerData.status.GetStatus(StatusType.DASH_SPEED).GetValue();
		AudioManager.instance.PlayOneShot(pc.dash, pc.transform.position);

		pc.glove.SetActive(false);
	}

	public override void Update(PlayerController pc)
	{
		if (currentTime > dashTime)
		{
			pc.ChangeState(PlayerController.PlayerState.Idle);
		}
		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		//base.End(pc);
		pc.dashEffect.enabled = false;
		pc.rigid.velocity = Vector3.zero;
		pc.coolTimeIsEnd = false;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		
	}
}
