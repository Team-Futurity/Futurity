using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Spawn)]
public class EnemySpawnState : UnitState<EnemyController>
{
	private float curTime = .0f;

	public override void Begin(EnemyController unit)
	{
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue();

		if (unit.atkCollider != null)
			unit.atkCollider.enabled = false;
		unit.enemyCollider.enabled = false;
		unit.copyMat.color = Color.black;
		unit.skinnedMeshRenderer.enabled = false;
		unit.spawnEffect.SetActive(true);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		unit.DelayChangeState(curTime, unit.maxSpawningTime, unit, EnemyController.EnemyState.Idle);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.enemyCollider.enabled = true;
		unit.chaseRange.enabled = true;
		unit.skinnedMeshRenderer.enabled = true;
		unit.spawnEffect.SetActive(false);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
