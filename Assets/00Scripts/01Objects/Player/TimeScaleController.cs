using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : Singleton<TimeScaleController>
{
	public GameObject rayPos;
	[HideInInspector] public Vector3 forward;
	public float distanceThreshold;
	public float originTimeScale = 1;
	private bool isAdjusting;
	private bool isReady;
	private float curTimeScale;
	private float adjustTime;

	private void Start()
	{
		isAdjusting = false;
		StartCoroutine(TimeScaleCoroutine());
	}

	private GameObject GetPos()
	{
		rayPos = GameObject.Find("Bip001 Pelvis");

		return rayPos;
	}

	public void SetTimeScale(float timeScale, float adjustTime, Vector3 forward)
	{
		curTimeScale = timeScale;
		this.adjustTime = adjustTime;
		this.forward = forward;
		isReady = true;
	}

	private void Update()
	{
		if (!isReady || isAdjusting) { return; }
		if (GetPos() == null) { return; }

		RaycastHit[] hits = Physics.RaycastAll(GetPos().transform.position, forward, distanceThreshold);
		foreach (var hit in hits)
		{
			if (hit.transform.CompareTag("Enemy"))
			{
				isAdjusting = true;
				isReady = false;
				return;
			}
		}
	}

	private IEnumerator TimeScaleCoroutine()
	{
		while (true)
		{
			if (isAdjusting)
			{
				Time.timeScale = curTimeScale;
				yield return new WaitForSeconds(adjustTime * Time.timeScale);
				Time.timeScale = originTimeScale;
				isAdjusting = false;
			}
			yield return null;
		}
	}
}
