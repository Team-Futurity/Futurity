using UnityEngine;

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

		unit.playerData.DisableAttackTime();

		unit.attackColliderChanger.UnlockColliderEnable();
		unit.autoTargetColliderChanger.UnlockColliderEnable();
		unit.attackColliderChanger.DisableAllCollider();
		unit.autoTargetColliderChanger.DisableAllCollider();
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
			if (attackNode.childNodes.Count == 0 && currentTime < attackNode.attackAfterDelay) { return; }
			//unit.StartNextComboAttack(unit.nextCombo, PlayerState.NormalAttack);
			if (!unit.NodeTransitionProc(unit.nextCombo, PlayerState.NormalAttack)) { /*unit.ChangeState(PlayerState.Idle);*/ return; }

			unit.nextCombo = PlayerInputEnum.None;
			unit.LockNextCombo(false);
			NextAttackState(unit, PlayerState.AttackDelay);
			return;
		}

		if (currentTime >= attackNode.attackAfterDelay)
		{
			SendAttackEndMessage(unit);

			unit.sariObject.OnStop();

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
		string msg = unit.GetInputData(unit.curCombo, true, unit.currentAttackState.ToString(), attackNode.name, "Complete").inputMsg;
		unit.attackEndEvent.Invoke(msg);
	}
}
