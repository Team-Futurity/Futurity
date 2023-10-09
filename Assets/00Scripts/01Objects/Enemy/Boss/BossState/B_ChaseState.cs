using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.Chase)]
public class B_ChaseState : UnitState<BossController>
{
	private float distance = .0f;

	public override void Begin(BossController unit)
	{
		unit.curState = BossController.BossState.Chase;
		unit.animator.SetBool(unit.moveAnim, true);
		unit.navMesh.enabled = true;
	}
	public override void Update(BossController unit)
	{
		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);

		if (distance < unit.meleeDistance)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.transform.LookAt(unit.target.transform);
			unit.ChangeState(BossController.BossState.T1_Melee);
		}
		else if (distance < unit.targetDistance)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.transform.LookAt(unit.target.transform);
			unit.ChangeState(BossController.BossState.T2_Ranged);
		}
		else if (distance > unit.targetDistance)
		{
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}

	public override void End(BossController unit)
	{
		unit.animator.SetBool(unit.moveAnim, false);
		unit.navMesh.enabled = false;
	}

	public override void FixedUpdate(BossController unit)
	{
	}

	public override void OnCollisionEnter(BossController unit, Collision collision)
	{
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
	}
}
