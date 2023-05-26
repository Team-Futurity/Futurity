using System;
using System.Collections;
using UnityEngine;

public class HitCountSystem : MonoBehaviour
{
	[SerializeField] private int maxHitCount = 999;
	[SerializeField] private float coolTime = 10;
	private bool isCounting = false;
	private float currentTime = 0;
	public NumberImageLoader hitCountUI;
 	[field: SerializeField] public int hitCount { get; private set; }

	private void Start()
	{
		hitCountUI.SetNumber(0);
	}

	public void AddHitCount(int count)
	{
		hitCount = Mathf.Clamp(hitCount + count, 0, maxHitCount);
		currentTime = 0;
		isCounting = true;
		hitCountUI.SetNumber(hitCount);
	}

	private void Update()
	{
		if (isCounting)
		{
			currentTime += Time.deltaTime;
			if(currentTime >= maxHitCount)
			{
				isCounting = false;
				currentTime = 0;
				hitCount = 0;
			}
		}
	}
}
