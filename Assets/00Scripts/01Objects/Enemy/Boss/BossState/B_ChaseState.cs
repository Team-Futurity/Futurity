using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.Chase)]
public class B_ChaseState : BossStateBase
{
	private int random = 0;
	private int dashPercentage = 60;
	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.Chase;
		unit.animator.SetBool(unit.moveAnim, true);
		unit.navMesh.enabled = true;

		random = Random.Range(1, 101);
	}
	public override void Update(BossController unit)
	{
		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);
		unit.transform.LookAt(unit.target.transform);

		if (random < dashPercentage)
		{
			unit.nextState = BossState.T0_Dash;
			unit.ChangeState(unit.nextState);
		}
		else
		{
			if (distance < unit.chaseDistance)
			{
				FDebug.Log(distance);
				unit.rigid.velocity = Vector3.zero;
				unit.activeDataSO.SetRandomNextState(unit);
				unit.ChangeState(unit.nextState);
			}
			else
				unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.animator.SetBool(unit.moveAnim, false);
		unit.rigid.velocity = Vector3.zero;
		unit.navMesh.enabled = false;
	}
}
