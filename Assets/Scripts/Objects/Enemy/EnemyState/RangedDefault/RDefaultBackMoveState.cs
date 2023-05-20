using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

[FSMState((int)EnemyController.EnemyState.RDefaultBackMove)]
public class RDefaultBackMoveState : UnitState<EnemyController>
{
	private float curTime;

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("RDefaultBackMove Begin");
		unit.rigid.AddForce(-GetAngle(unit, 60) * 800.0f, ForceMode.Impulse);
	}
	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, 0.4f, unit, EnemyController.EnemyState.RDefaultChase);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}
	public override void End(EnemyController unit)
	{
		//FDebug.Log("RDefaultBackMove End");
		unit.rigid.AddForce(-GetAngle(unit, -80) * 900.0f, ForceMode.Impulse);
		curTime = 0f;

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	private Vector3 GetAngle(EnemyController unit, float y)
	{
		Vector3 direction = unit.transform.forward;

		var quaternion = Quaternion.Euler(0, y, 0);
		Vector3 targetDir = quaternion * direction;

		return targetDir;
	}
}
