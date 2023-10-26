using Unity.VisualScripting;
using UnityEngine;

[FSMState((int)EnemyState.MoveIdle)]
public class EnemyMoveIdleState : RandomMoveBaseState
{

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, true);
		base.Begin(unit);
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		if (distance < 1.0f)
			unit.ChangeState(EnemyState.Idle);
	}
	public override void End(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, false);
		base.End(unit);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag(unit.playerTag))
		{
			unit.target = other.GetComponent<UnitBase>();
			unit.ChangeState(unit.UnitChaseState());
		}
	}
}
