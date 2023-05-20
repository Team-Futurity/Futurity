using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultChase)]
public class MiniDefaultChaseState : UnitState<EnemyController>
{
	private float curTime = .0f;
	private float distance;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Chase begin");
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.isChasing = true;
	}

	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;

		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);
		unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(unit.target.transform.position), 15.0f * Time.deltaTime);

		if(distance < unit.chaseDistance)
		{
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.DelayChangeState(curTime, unit.chaseDelayTime, unit, EnemyController.EnemyState.MiniDefaultAttack);
		}
		else if (distance > unit.chaseDistance)
			unit.transform.position += unit.transform.forward.normalized * unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime;
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("MiniDefault Chase end");
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.isChasing = false;
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
