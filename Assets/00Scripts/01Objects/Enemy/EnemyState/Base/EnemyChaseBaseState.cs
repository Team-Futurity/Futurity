using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyChaseBaseState : StateBase
{
	protected float distance = .0f;
	protected float attackRange;

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.navMesh.enabled = true;
	}
	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;

		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);
	}

	public override void End(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.navMesh.enabled = false;
		curTime = .0f;
	}
}
