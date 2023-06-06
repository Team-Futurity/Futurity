using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyChaseBaseState : UnitState<EnemyController>
{
	protected float distance = .0f;

	public override void Begin(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;

		/*unit.isChasing = true;*/
	}
	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;

		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);
	}
	public override void FixedUpdate(EnemyController unit)
	{

	}
	public override void End(EnemyController unit)
	{
		unit.animator.SetBool(unit.moveAnimParam, false);

		/*unit.isChasing = false;*/
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}
}
