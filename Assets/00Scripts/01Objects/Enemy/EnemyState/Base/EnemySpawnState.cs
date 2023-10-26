using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[FSMState((int)EnemyState.Spawn)]
public class EnemySpawnState : StateBase
{
	private float maxSpawningTime = 0.1f;
	private float walkDistance = 1.0f;

	private Color BeginColor = Color.black;
	private Color refColor = Color.black;
	private Vector3 targetPos;

	public override void Begin(EnemyController unit)
	{
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue();
		unit.navMesh.enabled = false;

		if (unit.atkCollider != null)
			unit.atkCollider.enabled = false;

		if (unit.ThisEnemyType == EnemyType.RangedDefault)
			unit.SettingProjectile();
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		unit.DelayChangeState(curTime, maxSpawningTime, unit, EnemyState.Idle);
	}
	public override void End(EnemyController unit)
	{
		unit.enemyCollider.enabled = true;
		unit.chaseRange.enabled = true;
		unit.animator.SetBool(unit.moveAnimParam, false);
		unit.rigid.velocity = Vector3.zero;
	}
}
