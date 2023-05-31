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
		//hitCountUI.SetNumber(0);
	}

	public void AddHitCount(int count)
	{
		SetHitCount(hitCount + count);
		currentTime = 0;
		isCounting = true;
	}

	private void SetHitCount(int number)
	{
		hitCount = Mathf.Clamp(number, 0, maxHitCount);

		if (hitCount != 0)
		{
			hitCountUI.SetNumber(hitCount);
		}
	}

	private void Update()
	{
		if (isCounting)
		{
			currentTime += Time.deltaTime;
			if(currentTime >= coolTime)
			{
				isCounting = false;
				currentTime = 0;
				SetHitCount(0);
			}
		}
	}
}
