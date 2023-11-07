using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
	[SerializeField] private Transform player;
	[SerializeField] private Transform enemies;
	[SerializeField] private GameObject indicatorEffect;
	private GameObject indicatorInstance;

	private void Start()
	{
		indicatorInstance = Instantiate(indicatorEffect, transform);
		indicatorInstance.SetActive(false);
	}

	public void ActivateIndicator(GameObject target = null)
	{
		if(target == null)
		{
			target = GetNearestEnemy().gameObject;

			if(target == null) { FDebug.LogWarning("Failed to Activate Indicator", GetType()); return; }
		}

		Vector3 dir = (target.transform.position - player.transform.position).normalized;

		indicatorInstance.transform.LookAt(dir);
		indicatorInstance.SetActive(true);
	}

	public void DeactiveIndicator()
	{
		indicatorInstance.SetActive(false);
	}

	private Transform GetNearestEnemy()
	{
		if(enemies == null) { return null; }

		var enemyTransforms = enemies.GetComponentsInChildren<Transform>();
		var orderedEnemies = enemyTransforms.Where(t => t.gameObject.activeSelf).OrderBy(enemy => (enemy.position - player.transform.position).sqrMagnitude).ToArray();

		if(orderedEnemies.Length == 0) { return null; }

		return orderedEnemies[0];
	}

}
