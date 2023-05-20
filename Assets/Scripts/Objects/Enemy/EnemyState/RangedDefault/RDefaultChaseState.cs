using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.RDefaultChase)]
public class RDefaultChaseState : UnitState<EnemyController>
{
	private float curTime = .0f;
	private float distance;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("RDefault chase Begin");
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.isChasing = true;
	}

	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;
		unit.transform.LookAt(unit.target.transform.position);
		//unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), 15.0f * Time.deltaTime);

		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);

		if(distance < unit.chaseDistance * 0.5f)
		{
			unit.ChangeState(EnemyController.EnemyState.RDefaultBackMove);
		}
		else if (distance < unit.chaseDistance)
		{
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.DelayChangeState(curTime, unit.chaseDelayTime, unit, EnemyController.EnemyState.RDefaultAttack);
		}
		else if (distance > unit.chaseDistance)
		{
			unit.transform.position += unit.transform.forward * unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime;
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("RDefault chase End");
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.isChasing = false;
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
