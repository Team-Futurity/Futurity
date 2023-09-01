using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.XR;
using static PlayerController;

[FSMState((int)PlayerState.AttackAfterDelay)]
public class PlayerAttackAfterDelayState : PlayerComboAttackState
{
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
		if(unit.nextCombo != PlayerInputEnum.None)
		{
			//unit.StartNextComboAttack(unit.nextCombo, PlayerState.NormalAttack);
			if (!unit.NodeTransitionProc(unit.nextCombo, PlayerState.NormalAttack)) { /*unit.ChangeState(PlayerState.Idle);*/ return; }

			if(attackNode.name == "JJJ")
			{
				Debug.Log("");
			}

			unit.nextCombo = PlayerInputEnum.None;
			unit.LockNextCombo(false);
			NextAttackState(unit, PlayerState.AttackDelay);
			return;
		}

		if (currentTime >= attackNode.attackAfterDelay)
		{
			SendAttackEndMessage(unit);

			unit.RotatePlayer(unit.lastMoveDir);
			unit.ChangeState(PlayerState.Idle);

			return;
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		
	}

	private void SendAttackEndMessage(PlayerController unit)
	{
		string msg = unit.GetInputData(unit.curCombo, true, unit.currentAttackState.ToString(), attackNode.name, "Complete");
		unit.attackEndEvent.Invoke(msg);
	}
}
