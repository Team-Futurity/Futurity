using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtaCore : CoreAbility
{
	[SerializeField]
	private float colliderRadius = .0f;

	[SerializeField]
	private float enemyCheckCycle = .0f;

	[SerializeField]
	private float colliderActiveTime = .0f;

	[SerializeField]
	private LayerMask targetLayer;

	[SerializeField]
	private GameObject afterDeathObj;

	protected override void OnPartAbility(UnitBase enemy)
	{
		var deathObj = Instantiate(afterDeathObj);
		var execute = deathObj.AddComponent<ExecuteAfterDeath>();

		execute.SetCollider(colliderRadius, enemyCheckCycle, colliderActiveTime, targetLayer);

		TryGetComponent<EnemyController>(out var enemyController);
		enemyController.onDeathEvent.AddListener(execute.AddEnemyEvent);
	}
}
