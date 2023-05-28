using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerState.AttackAfterDelay)]
public class PlayerAttackAfterDelayState : PlayerAttackBaseState
{
	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);

		unit.nextCombo = PlayerInput.None;
	}

	public override void FixedUpdate(PlayerController unit)
	{

	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{

	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);
		if(unit.nextCombo != PlayerInput.None)
		{
			AttackNode node = unit.FindInput(unit.nextCombo);

			if (node == null) { unit.nextCombo = PlayerInput.None; return; }
				
			unit.curNode = node;
			unit.curCombo = node.command;
			unit.currentAttackState = PlayerState.NormalAttack;

			NextAttackState(unit, PlayerState.AttackDelay);
			return;
		}

		if (currentTime >= attackNode.attackAfterDelay)
		{
			unit.ChangeState(PlayerState.Idle);

			return;
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
