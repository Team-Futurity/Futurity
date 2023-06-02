using UnityEngine;
using UnityEngine.Windows;
using static PlayerController;

[FSMState((int)PlayerState.AttackAfterDelay)]
public class PlayerAttackAfterDelayState : PlayerAttackBaseState
{
	bool comboIsEnd = false;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);
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
			//unit.StartNextComboAttack(unit.nextCombo, PlayerState.NormalAttack);
			if (!unit.NodeTransitionProc(unit.nextCombo, PlayerState.NormalAttack)) { /*unit.ChangeState(PlayerState.Idle);*/ return; }

			FDebug.Log("NEXT");

			unit.nextCombo = PlayerInput.None;
			unit.LockNextCombo(false);
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
