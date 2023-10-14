using UnityEngine;
using static PlayerController;

public class PlayerAttackBaseState : UnitState<PlayerController>
{
	protected float currentTime;

	public override void Begin(PlayerController unit)
	{
		currentTime = 0;
	}

	public override void Update(PlayerController unit)
	{
		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController unit)
	{
		unit.autoTargetColliderChanger.DisableAllCollider();
		unit.attackColliderChanger.DisableAllCollider();

		unit.animator.SetBool(unit.IsAttackingAnimKey, false);
		unit.animator.SetInteger(unit.currentAttackAnimKey, NullState);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}

	public override void OnCollisionStay(PlayerController unit, Collision collision)
	{

	}
}
