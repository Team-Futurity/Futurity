using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteChaseState : EnemyChaseBaseState
{
	private float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("RDefault chase Begin");

		base.Begin(unit);
	}

	public override void Update(EnemyController unit)
	{
		base.Update(unit);

		unit.transform.LookAt(unit.target.transform.position);
		//unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), unit.turnSpeed * Time.deltaTime);


		if (distance < unit.attackRange * 0.5f)
		{
			unit.ChangeState(EnemyController.EnemyState.EliteMeleeAttack);
		}
		else if (distance < unit.attackRange)
		{
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.navMesh.enabled = false;
			unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.EliteRangedAttack);
		}
		else if (distance > unit.attackRange)
		{
			//unit.transform.position += unit.transform.forward * unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime;
			unit.navMesh.enabled = true;
			unit.navMesh.SetDestination(unit.target.transform.position);
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("RDefault chase End");

		base.End(unit);
		curTime = 0f;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		/*		if (other.CompareTag(unit.playerTag))
				{
					//FDebug.Log("RDefault Chase Trigger");
					unit.rigid.velocity = Vector3.zero;
					unit.ChangeState(EnemyController.EnemyState.RDefaultBackMove);
				}*/
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{
	}
}
