using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Spawn)]
public class EnemySpawnState : UnitState<EnemyController>
{
	private float curTime = .0f;
	private Color BeginColor = Color.black;
	private Color refColor = Color.black;
	private Vector3 targetPos;

	public override void Begin(EnemyController unit)
	{
		unit.navMesh.speed = unit.enemyData.status.GetStatus(StatusType.SPEED).GetValue();

		if (unit.atkCollider != null)
			unit.atkCollider.enabled = false;
		unit.enemyCollider.enabled = false;
		unit.copyUMat.color = BeginColor;
		unit.animator.SetBool(unit.moveAnimParam, true);
		unit.spawnEffect.SetActive(true);

		targetPos = unit.transform.position + unit.transform.forward * unit.walkDistance;
	}

	public override void Update(EnemyController unit)
	{
		if (true)
			return;
		
		curTime += Time.deltaTime;

		if (refColor.a > 0f)
			refColor.a -= curTime * 0.005f;
		unit.copyUMat.SetColor(unit.matColorProperty, refColor);
		unit.navMesh.SetDestination(targetPos);
		unit.DelayChangeState(curTime, unit.maxSpawningTime, unit, EnemyController.EnemyState.Idle);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		unit.enemyCollider.enabled = true;
		unit.chaseRange.enabled = true;
		unit.animator.SetBool(unit.moveAnimParam, false);
		//unit.skinnedMeshRenderer.enabled = true;
		unit.rigid.velocity = Vector3.zero;
		
		// spawnEffect Reset
		unit.spawnEffect.transform.SetParent(unit.transform);
		unit.spawnEffect.SetActive(false);

/*		unit.manager = EnemyManager.Instance;
		unit.effectManager = EnemyEffectManager.Instance;*/

		unit.effectManager.CopyEffect(unit);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
