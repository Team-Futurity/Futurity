using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HitCountSystem : MonoBehaviour
{
	[SerializeField] private int maxHitCount = 999;
	[SerializeField] private float coolTime = 10;
	private bool isCounting = false;
	private float currentTime = 0;
 	[field: SerializeField] public int hitCount { get; private set; }

	[HideInInspector] public UnityEvent<int> updateHitCount;

	public void AddHitCount(int count)
	{
		SetHitCount(hitCount + count);
		currentTime = 0;
		isCounting = true;
	}

	private void SetHitCount(int number)
	{
		hitCount = Mathf.Clamp(number, 0, maxHitCount);

		if (number == 0) return;

		updateHitCount?.Invoke(hitCount);
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
