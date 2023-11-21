using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.Idle)]
public class PlayerIdleState : UnitState<PlayerController>
{
	private const string IdleAdditionalAnimKey = "AdditionalIdleNumber";
	private const string TriggerAdditionalAnimKey = "TriggerAdditionalIdle";
	private const float TargetTime = 12f;
	private const int AnimationCount = 2;
	private const float RandomRange = 3f;
	private float currentTime;
	private float currentRange;

	public override void Begin(PlayerController pc)
	{
		pc.rigid.velocity = Vector3.zero;
		pc.animator.SetBool(pc.IsAttackingAnimKey, false);
		pc.rmController.SetRootMotion("Idle");

		if (pc.moveIsPressed && !pc.lockAllInput)
		{
			pc.ChangeState(PlayerState.Move);
		}

		currentTime = 0;
		currentRange = Random.Range(TargetTime - RandomRange, TargetTime + RandomRange);

		pc.UnlockInput();
		pc.attackColliderChanger.UnlockColliderEnable();
		pc.autoTargetColliderChanger.UnlockColliderEnable();
		pc.attackColliderChanger.DisableAllCollider();
		pc.autoTargetColliderChanger.DisableAllCollider();
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime >= currentRange)
		{
			int rand = Random.Range(0, AnimationCount);

			pc.animator.SetInteger(IdleAdditionalAnimKey, rand);
			pc.animator.SetTrigger(TriggerAdditionalAnimKey);

			currentTime = 0;
			currentRange = Random.Range(TargetTime - RandomRange, TargetTime + RandomRange);
		}

		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
		if (!unit.playerData.isKnockbaking)
		{
			unit.rigid.velocity = Vector3.zero;
		}
	}

	public override void End(PlayerController pc)
	{

	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
