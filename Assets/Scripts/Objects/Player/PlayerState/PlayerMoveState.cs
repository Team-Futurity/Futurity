using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.Move)]
public class PlayerMoveState : UnitState<PlayerController>
{
	public override void Begin(PlayerController pc)
	{
		//base.Begin(pc);
		pc.animator.SetBool("Move", true);
	}

	public override void Update(PlayerController pc)
	{
		if(pc.moveDir == Vector3.zero)
		{
			pc.ChangeState(PlayerController.PlayerState.Idle);
		}
	}

	public override void FixedUpdate(PlayerController pc)
	{
		Vector3 rotVec = Quaternion.AngleAxis(45, Vector3.up) * pc.moveDir;

		//pc.transform.rotation = Quaternion.Lerp(pc.transform.rotation, Quaternion.LookRotation(rotVec), 1.0f * Time.deltaTime);
		pc.transform.rotation = Quaternion.LookRotation(rotVec);
		pc.transform.position += rotVec.normalized * pc.playerData.Speed * Time.deltaTime;
	}

	public override void End(PlayerController pc)
	{
		//base.End(pc);
		pc.animator.SetBool("Move", false);
		pc.rigid.velocity = Vector3.zero;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		throw new System.NotImplementedException();
	}
}
