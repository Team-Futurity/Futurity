using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.AutoMove)]
public class EnemyAutoMoveState : StateBase
{
	private Vector3 targetPos;


	public void SetTarget(Vector3 targetPos)
	{
		this.targetPos = targetPos;
	}

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.navMesh.enabled = true;
	}

	public override void Update(EnemyController unit)
	{
		if (unit.transform.position != targetPos)
			unit.navMesh.SetDestination(targetPos);
		else if (unit.target != null)
			unit.ChangeState(unit.UnitChaseState());
		else
			unit.ChangeState(EnemyState.Idle);
	}

	public override void End(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.navMesh.enabled = false;
	}
}
