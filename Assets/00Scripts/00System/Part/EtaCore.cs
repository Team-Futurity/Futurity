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

	[SerializeField]
	private GameObject afterDeathObjEnabled;

	protected override void OnPartAbility(UnitBase enemy)
	{
		if(enemy.status.GetStatus(StatusType.CURRENT_HP).GetValue() > 0) { return; }

		var deathObj = Instantiate(afterDeathObj, enemy.gameObject.transform.position, enemy.gameObject.transform.rotation);
		deathObj.transform.eulerAngles = new Vector3(-90, deathObj.transform.rotation.eulerAngles.y, 0);
		var execute = deathObj.AddComponent<ExecuteAfterDeath>();

		execute.SetCollider(colliderRadius, enemyCheckCycle, colliderActiveTime, targetLayer);

		execute.AddEnemyEvent(afterDeathObjEnabled);
	}
}
