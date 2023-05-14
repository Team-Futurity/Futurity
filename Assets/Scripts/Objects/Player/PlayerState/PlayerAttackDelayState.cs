using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerState.AttackDelay)]
public class PlayerAttackDelayState : UnitState<PlayerController>
{
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
		
	}

	public override void FixedUpdate(PlayerController unit)
	{
		throw new System.NotImplementedException();
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		throw new System.NotImplementedException();
	}

	public override void Update(PlayerController unit)
	{
		if (currentTime > attackNode.skillSpeed * animRatio)
		{
			unit.curNode = unit.comboTree.top;
			unit.ChangeState(PlayerState.Idle);
		}
		currentTime += Time.deltaTime;
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		throw new System.NotImplementedException();
	}
}
