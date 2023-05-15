using UnityEngine;
using UnityEngine.XR;
using static PlayerController;

[FSMState((int)PlayerState.AttackAfterDelay)]
public class PlayerAttackAfterDelayState : UnitState<PlayerController>
{
	private readonly string IsAtttackingAnimKey = "IsAttacking";
	private float currentTime;
	protected AttackNode attackNode;

	public override void Begin(PlayerController unit)
	{
		attackNode = unit.curNode;
		currentTime = 0;

		FDebug.Log("CurrentState : AttackAfter");
	}

	public override void End(PlayerController unit)
	{
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
		if(unit.nextCombo != PlayerInput.None)
		{
			AttackNode node = unit.FindInput(unit.nextCombo);

			if (node == null) { return; }
				
			unit.curNode = node;
			unit.curCombo = node.command;
			unit.currentAttackState = PlayerState.NormalAttack;

			unit.ChangeState(PlayerState.AttackDelay);
			return;
		}

		if (currentTime > attackNode.attackAfterDelay)
		{
			unit.curNode = unit.comboTree.top;
			unit.animator.SetBool(IsAtttackingAnimKey, false);
			unit.ChangeState(PlayerState.Idle);

			return;
		}
		currentTime += Time.deltaTime;
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
