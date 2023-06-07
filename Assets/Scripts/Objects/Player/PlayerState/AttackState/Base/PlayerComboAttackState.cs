using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerComboAttackState : PlayerAttackBaseState
{
	protected bool isNextAttackState;
	protected AttackNode attackNode;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);

		isNextAttackState = false;
		attackNode = unit.curNode;
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);
	}

	public override void End(PlayerController unit)
	{
		if (!isNextAttackState)
		{
			unit.ResetCombo();

			base.End(unit);
		}
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);
	}

	public override void OnCollisionStay(PlayerController unit, Collision collision)
	{
		base.OnCollisionStay(unit, collision);
	}

	public virtual void NextAttackState(PlayerController unit, PlayerState nextState)
	{
		isNextAttackState = true;
		unit.ChangeState(nextState);
	}
}
