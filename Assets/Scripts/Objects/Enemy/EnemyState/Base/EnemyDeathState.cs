using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Death)]
public class EnemyDeathState : UnitState<EnemyController>
{
	private float curTime = 0f;

	public override void Begin(EnemyController unit)
	{
		if (unit.isClustering)
			unit.clusteringManager.EnemyDeclutter(unit.clusterNum);
		unit.hpBar.copySlider.gameObject.SetActive(false);
		unit.enemyCollider.enabled = false;
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		if(curTime > 1.0f)
		{
			unit.gameObject.SetActive(false);
		}
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{

	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
