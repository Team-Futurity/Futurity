using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectIndicator : MonoBehaviour
{
	[Header("Reference")]
	[SerializeField] private Transform player;
	[SerializeField] private Transform enemies;

	[Header("Effect")]
	[SerializeField] private GameObject defaultIndicatorEffect;
	[SerializeField] private GameObject otherIndicatorEffect;

	[Header("Distance")]
	[SerializeField] private float invisibleDistance;
	[SerializeField] private float maxVisibleDistance;

	[Header("Size")]
	[SerializeField] private AnimationCurve sizeCurve;
	[SerializeField] private float minSize;
	[SerializeField] private float maxSize;

	private GameObject defaultIndicatorInstance;
	private GameObject otherIndicatorInstance;
	private GameObject currentIndicator;
	private bool isActive;
	public bool IsActive => isActive;

	private IEnumerator enemyTracking;

	private void Start()
	{
		defaultIndicatorInstance = Instantiate(defaultIndicatorEffect, transform);
		otherIndicatorInstance = Instantiate(otherIndicatorEffect, transform);
		defaultIndicatorInstance.SetActive(false);
		otherIndicatorInstance.SetActive(false);
	}

	public void ActivateIndicator(GameObject target = null)
	{
		isActive = true;

		currentIndicator = target != null ? otherIndicatorInstance : defaultIndicatorInstance;	

		currentIndicator.SetActive(true);

		enemyTracking = EnemyTrackingCoroutine(currentIndicator, target);
		StartCoroutine(enemyTracking);
	}

	public void DeactiveIndicator()
	{
		if (enemyTracking == null)
		{
			return;
		}
		
		isActive = false;
		if (currentIndicator != null)
		{
			currentIndicator.SetActive(false);
			currentIndicator = null;
		}
		StopCoroutine(enemyTracking);
	}

	private Transform GetNearestEnemy()
	{
		if(enemies == null) { return null; }

		List<Transform> enemyTransforms = new List<Transform>();

		foreach (Transform child in enemies)
		{
			enemyTransforms.Add(child);
		}

		var orderedEnemies = enemyTransforms.Where(t => t.gameObject.activeSelf).OrderBy(enemy => (enemy.position - player.transform.position).sqrMagnitude).ToArray();

		if(orderedEnemies.Length == 0) { return null; }

		return orderedEnemies[0];
	}

	private void SetSize(float distance, GameObject indicator)
	{
		float distanceRatio = (distance - invisibleDistance) / (maxVisibleDistance - invisibleDistance);
		float currentCurvePoint = sizeCurve.Evaluate(distanceRatio);
		float size = Mathf.Clamp(currentCurvePoint * maxSize, minSize, maxSize);

		indicator.transform.localScale = new Vector3(size, size, size);
	}

	private IEnumerator EnemyTrackingCoroutine(GameObject indicator, GameObject defaultTarget = null)
	{
		while(isActive)
		{
			Transform target = defaultTarget?.transform;
			if (target == null)
			{
				target = GetNearestEnemy();

				if (target == null) { FDebug.LogWarning("Failed to Activate Indicator", GetType()); yield return null; continue; }
			}

			Vector3 dir = (target.position - player.transform.position).normalized;

			indicator.transform.rotation = Quaternion.LookRotation(dir);
			indicator.transform.eulerAngles = new Vector3(0, indicator.transform.eulerAngles.y, 0);

			float distance = (target.position - player.transform.position).magnitude;

			SetSize(distance, indicator);

			bool isVisible = distance > invisibleDistance;
			indicator.SetActive(isVisible);

			yield return null;
		}

		indicator.SetActive(false);
	}
}
