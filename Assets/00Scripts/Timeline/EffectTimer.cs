using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTimer : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private Material auraMat;

	[Header("Delay Time")] 
	[SerializeField, Range(0, 1)] private float delayTime = 0.5f;
	
	[HideInInspector] public bool isActive = true;
	private float unScaldTime;
	private IEnumerator initUnScaldTime;
	
	public void StartInitUnScaldTime()
	{
		unScaldTime = 0.0f;

		initUnScaldTime = InitUnScaldTime();
		StartCoroutine(initUnScaldTime);
	}

	public void StopInitTimer()
	{
		if (initUnScaldTime == null)
		{
			return;
		}

		isActive = false;
		unScaldTime = 0.0f;
		
		StopCoroutine(InitUnScaldTime());
	}
	
	private IEnumerator InitUnScaldTime()
	{
		while (isActive == true)
		{
			unScaldTime += Time.unscaledDeltaTime * delayTime;
			auraMat.SetFloat("_UnTimeScale", unScaldTime);

			yield return null;
		}
	}
}
