using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.RDefaultChase)]
public class RDefaultChaseState : UnitState<EnemyController>
{
	private float curTime = 0f;
	private float distance;

/*	private float xPos = 0f;
	private float zPos = 0f;
	private float xPosSpeed = 0f;*/

	public override void Begin(EnemyController unit)
	{
		FDebug.Log("RDefault chase Begin");
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.chaseRange.enabled = false;
		unit.atkRange.enabled = true;
		unit.isChasing = true;
	}

	public override void Update(EnemyController unit)
	{
		if (unit.target == null)
			return;
		unit.transform.LookAt(unit.target.transform.position);
		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);

		if (distance < unit.rangedDistance)
		{
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.DelayChangeState(curTime, unit.attackSetTime, unit, EnemyController.EnemyState.RDefaultAttack);
		}
		else if (distance > unit.rangedDistance + 1.0f)
		{
			unit.transform.position += unit.transform.forward * unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue() * Time.deltaTime;
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{
/*		xPosSpeed += Time.deltaTime * 20.0f;
		xPos = Mathf.Cos(xPosSpeed) * 3.0f;
		zPos += Time.deltaTime * 13.0f;
		unit.transform.position = new Vector3(xPos, 0f, zPos);*/

		/*		unit.transform.position = Vector3.MoveTowards(unit.transform.position,
				unit.RangedBackPos.transform.position,
				unit.enemyData.Speed * Time.deltaTime);*/
	}

	public override void End(EnemyController unit)
	{
		FDebug.Log("RDefault chase End");
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.isChasing = false;
		unit.atkRange.enabled = false;
		/*		curTime = 0f;
				xPos = 0f;
				zPos = 0f;*/
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{
		if (other.CompareTag("Player"))
		{
			FDebug.Log("RDefault Chase Trigger");
			unit.rigid.velocity = Vector3.zero;
			unit.ChangeState(EnemyController.EnemyState.RDefaultBackMove);
		}
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{
		//throw new System.NotImplementedException();
	}
}
