using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T3_Move)]
public class T3_MoveState : UnitState<BossController>
{
	private float distance = 0;
	public override void Begin(BossController unit)
	{
		unit.navMesh.enabled = true;
		unit.curState = BossController.BossState.T3_Move;
	}
	public override void Update(BossController unit)
	{
		distance = Vector3.Distance(unit.transform.position, unit.type3StartPos.position);
		unit.navMesh.SetDestination(unit.type3StartPos.position);

		if(distance < 0.1f)
		{
			unit.transform.rotation = unit.type3StartPos.rotation;
			unit.ChangeState(BossController.BossState.T3_Laser);
		}
	}


	public override void End(BossController unit)
	{
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
