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
	[SerializeField] private GameObject defualtIndicatorEffect;
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
	private bool isActive;

	private void Start()
	{
		defaultIndicatorInstance = Instantiate(defualtIndicatorEffect, transform);
		otherIndicatorInstance = Instantiate(defualtIndicatorEffect, transform);
		defaultIndicatorInstance.SetActive(false);
		otherIndicatorInstance.SetActive(false);

		ActivateIndicator();
	}

	public void ActivateIndicator(GameObject target = null)
	{
		isActive = true;

		GameObject currentIndicator = target != null ? otherIndicatorInstance : defaultIndicatorInstance;	

		currentIndicator.SetActive(true);
		
		StartCoroutine(EnemyTrackingCoroutine(currentIndicator, target));
	}

	public void DeactiveIndicator()
	{
		isActive = false;
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

			float distance = (target.position - player.transform.position).magnitude;

			SetSize(distance, indicator);

			bool isVisible = distance > invisibleDistance;
			indicator.SetActive(isVisible);

			yield return null;
		}

		indicator.SetActive(false);
	}
}
