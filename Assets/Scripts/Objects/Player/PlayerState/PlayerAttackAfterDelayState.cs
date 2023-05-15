using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerState.AttackAfterDelay)]
public class PlayerAttackAfterDelayState : UnitState<PlayerController>
{
	private readonly string IsAtttackingAnimKey = "IsAttacking";
	private float currentTime;
	protected AttackNode attackNode;

	// 임시 변수
	public float animRatio = 0.3f;

	public override void Begin(PlayerController unit)
	{
		attackNode = unit.curNode;
		currentTime = 0;
	}

	public override void End(PlayerController unit)
	{
		unit.animator.SetBool(IsAtttackingAnimKey, false);
	}

	public override void FixedUpdate(PlayerController unit)
	{

	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{

	}

	public override void Update(PlayerController unit)
	{
		if (currentTime > attackNode.attackAfterDelay)
		{
			unit.curNode = unit.comboTree.top;
			unit.ChangeState(PlayerState.Idle);
		}
		currentTime += Time.deltaTime;
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
