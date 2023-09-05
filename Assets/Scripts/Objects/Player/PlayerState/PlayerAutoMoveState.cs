using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.AutoMove)]
public class PlayerAutoMoveState : UnitState<PlayerController>
{
	private readonly string MoveAnimKey = "Move";
	private float targetTime;
	private float curTime;
	private Vector3 targetPos;
	private Vector3 initialPos;

	public void SetAutoMove(Vector3 worldPos, float time)
	{
		targetPos = worldPos;
		targetTime = Mathf.Clamp(time, 0, float.MaxValue);
	}

	public override void Begin(PlayerController pc)
	{
		pc.animator.SetBool(MoveAnimKey, true);
		pc.animator.SetBool(pc.IsAttackingAnimKey, false);
		pc.rmController.SetRootMotion("Move");
		initialPos = pc.transform.position;
		curTime = 0;
		t = 0;
	}

	public override void Update(PlayerController pc)
	{

	}

	public override void FixedUpdate(PlayerController pc)
	{
		curTime += Time.fixedDeltaTime;
		Vector3 velo = Vector3.zero;
		float smoothT = Mathf.SmoothStep(0f, 1f, curTime / targetTime);
		// pc.transform.position = Vector3.SmoothDamp(pc.transform.position, targetPos, ref velo, targetTime * Time.deltaTime);
		pc.transform.position = Vector3.Lerp(initialPos, targetPos, smoothT);

		if (curTime >= targetTime)
		{
			pc.ChangeState(PlayerState.Idle);
		}
	}

	public override void End(PlayerController pc)
	{
		//base.End(pc);
		pc.animator.SetBool(MoveAnimKey, false);
		pc.rigid.velocity = Vector3.zero;
		pc.transform.position = targetPos;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		
	}
}