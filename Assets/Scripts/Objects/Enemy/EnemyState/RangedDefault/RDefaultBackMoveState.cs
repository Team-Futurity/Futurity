using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.RDefaultBackMove)]
public class RDefaultBackMoveState : UnitState<EnemyController>
{
	private float curTime = 0f;
	private float distance;

	private float xPos = 0f;
	private float zPos = 0f;
	private float xPosSpeed = 0f;

	public override void Begin(EnemyController unit)
	{
		FDebug.Log("RDefaultBackMove Begin");
	}
	public override void Update(EnemyController unit)
	{
		distance = Vector3.Distance(unit.transform.position, unit.target.transform.position);

	}
	public override void FixedUpdate(EnemyController unit)
	{
		xPosSpeed += Time.deltaTime * 20.0f;
		xPos = Mathf.Cos(xPosSpeed) * 3.0f;
		zPos += Time.deltaTime * 13.0f;
		unit.transform.position = new Vector3(xPos, 0f, -zPos);

		if(distance >= unit.rangedDistance)
		{
			curTime += Time.deltaTime;
			unit.rigid.velocity = Vector3.zero;
			unit.DelayChangeState(curTime, unit.attackSetTime, unit, EnemyController.EnemyState.RDefaultChase);
		}
	}
	public override void End(EnemyController unit)
	{
		FDebug.Log("RDefaultBackMove End");
	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}
}
