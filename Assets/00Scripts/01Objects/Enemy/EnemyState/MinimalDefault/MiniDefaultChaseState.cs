using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultChase)]
public class MiniDefaultChaseState : EnemyChaseBaseState
{
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Chase begin");

		base.Begin(unit);
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		//unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), unit.turnSpeed * Time.deltaTime);
		unit.transform.LookAt(unit.target.transform.position);

		if (distance < unit.attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.ChangeState(EnemyController.EnemyState.MiniDefaultDelay);
		}
		else if (distance > unit.attackRange)
		{
			//unit.transform.position += unit.transform.forward.normalized * unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime;
			unit.navMesh.enabled = true;
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Chase end");

		base.End(unit);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
