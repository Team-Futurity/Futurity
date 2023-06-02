using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MDefaultChase)]
public class MDefaultChaseState : EnemyChaseBaseState
{
	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MDefault Chase begin");

		base.Begin(unit);
	}
	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		//unit.transform.rotation = Quaternion.LookRotation(unit.target.transform.position);
		//unit.transform.LookAt(unit.target.transform.position);
		//unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), unit.turnSpeed * Time.deltaTime);
		if (distance < unit.attackRange)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.ChangeState(EnemyController.EnemyState.MDefaultAttack);
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
		//FDebug.Log("MDefault Chase end");

		base.End(unit);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
