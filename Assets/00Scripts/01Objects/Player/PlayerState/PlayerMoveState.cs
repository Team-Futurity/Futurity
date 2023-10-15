using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.Move)]
public class PlayerMoveState : UnitState<PlayerController>
{
	private readonly string MoveAnimKey = "Move";

	public override void Begin(PlayerController pc)
	{
		//base.Begin(pc);
		pc.animator.SetBool(MoveAnimKey, true);
		pc.animator.SetBool(pc.IsAttackingAnimKey, false);
		pc.rmController.SetRootMotion("Move");
	}

	public override void Update(PlayerController pc)
	{
		if(!pc.moveIsPressed)
		{
			if(pc.IsCurrentState(PlayerState.Move))
			{
				pc.ChangeState(PlayerState.Idle);
			}
			else
			{
				pc.RemoveSubState();
			}
		}
	}

	public override void FixedUpdate(PlayerController pc)
	{
		var rotVec = pc.RotatePlayer(pc.moveDir, true);

		if (Physics.Raycast(pc.transform.position, pc.transform.forward, out RaycastHit hit, pc.stopDistance, 1 << 6))
		{
			Vector3 cross = Vector3.Cross(pc.transform.forward, hit.transform.forward);
			float dot = Mathf.Abs(Vector3.Dot(pc.transform.forward, hit.transform.forward));
			if (dot < 0.1e-5 || dot == 1)
			{
				pc.animator.SetBool(MoveAnimKey, false);
			}
		}
		else
		{
			pc.animator.SetBool(MoveAnimKey, true);
			pc.transform.position += pc.playerData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime * rotVec.normalized;
		}
		
	}

	public override void End(PlayerController pc)
	{
		//base.End(pc);
		pc.animator.SetBool(MoveAnimKey, false);
		pc.rigid.velocity = Vector3.zero;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		
	}
}