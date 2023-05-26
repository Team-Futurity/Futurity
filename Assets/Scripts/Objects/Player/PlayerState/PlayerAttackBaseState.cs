using UnityEngine;
using static PlayerController;

public class PlayerAttackBaseState : UnitState<PlayerController>
{
	protected bool isNextAttackState;
	protected AttackNode attackNode;
	protected float currentTime;

	public override void Begin(PlayerController unit)
	{
		isNextAttackState = false;

		// node
		if(unit.IsCurrentState(PlayerState.AttackDelay))
		{
			unit.curNode.Copy(unit.curNode);
		}
		
		attackNode = unit.curNode;

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
		if(!isNextAttackState)
		{
			unit.curNode = unit.comboTree.top;

			unit.autoTargetCollider.radiusCollider.enabled = false;
			unit.attackCollider.radiusCollider.enabled = false;

			unit.animator.SetBool(unit.IsAttackingAnimKey, false);
			unit.animator.SetInteger(unit.currentAttackAnimKey, NullState);
		}
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

	public virtual void NextAttackState(PlayerController unit, PlayerState nextState)
	{
		isNextAttackState = true;
		unit.ChangeState(nextState);	
	}
}
