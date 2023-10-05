using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.Death)]
public class EnemyDeathState : UnitState<EnemyController>
{
	private float curTime = 0f;
	private Color refColor = new Color(0f, 0f, 0f, 0f);

	public override void Begin(EnemyController unit)
	{
		if (unit.isClustering)
		{
			ClusterManager.Instance.EnemyDeCluster(unit.clusterNum);
		}
		
		unit.rigid.constraints = RigidbodyConstraints.FreezeAll;
		unit.animator.SetTrigger(unit.deadAnimParam);
		unit.enemyCollider.enabled = false;
		
		unit.OnDisableEvent();
		unit.onDeathEvent?.Invoke();
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;

		if (refColor.a < 1f)
			refColor.a += curTime * 0.005f;
		unit.copyUMat.SetColor(unit.matColorProperty, refColor);

		if (curTime > unit.deathDelay)
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
