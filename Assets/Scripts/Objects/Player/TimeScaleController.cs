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

	public void SetTimeScale(float timeScale, float adjustTime, Vector3 forward)
	{
		curTimeScale = timeScale;
		this.adjustTime = adjustTime;
		this.forward = forward;
		isReady = true;
	}

	private void Update()
	{
		if(!isReady || isAdjusting) { return; }
		if(rayPos == null) { return; }
		
		RaycastHit[] hits = Physics.RaycastAll(rayPos.transform.position, forward, distanceThreshold);
		foreach(var hit in hits)
		{
			if(hit.transform.CompareTag("Enemy"))
			{
				isAdjusting = true;
				isReady = false;
				FDebug.Log("1");
				return;
			}
		}
	}

	private IEnumerator TimeScaleCoroutine()
	{
		while (true) { 
			if(isAdjusting)
			{
				Time.timeScale = curTimeScale;
				FDebug.Log("2");
				yield return new WaitForSeconds(adjustTime * Time.timeScale);
				Time.timeScale = originTimeScale;
				FDebug.Log("3");
				isAdjusting = false;
			}
			yield return null;
		}
	}
}
