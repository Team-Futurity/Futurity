using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTimer : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private Material alphaMat;
	[SerializeField] private Material betaMat;

	[Header("컷 씬 타입")] 
	[SerializeField] private ECutSceneType type;
	
	[Header("Delay Time")] 
	[SerializeField, Range(0, 1)] private float delayTime = 0.5f;
	
	[HideInInspector] public bool isActive = true;
	private float unScaldTime;
	private IEnumerator initUnScaldTimeForAlpha;
	private IEnumerator initUnScaldTimeForBeta;
	
	public void StartInitUnScaldTime(ECutSceneType cutSceneType)
	{
		unScaldTime = 0.0f;
		isActive = true;

		if (type == ECutSceneType.ACTIVE_ALPHA)
		{
			initUnScaldTimeForAlpha = InitUnScaldTimeForAlpha();
			StartCoroutine(initUnScaldTimeForAlpha);
		}
		else
		{
			initUnScaldTimeForBeta = InitUnScaldTimeForBeta();
			StartCoroutine(initUnScaldTimeForBeta);
		}
	}

	public void StopInitTimer()
	{
		if (initUnScaldTimeForAlpha == null)
		{
			StopCoroutine(InitUnScaldTimeForAlpha());
		}
		else if (initUnScaldTimeForBeta == null)
		{
			StopCoroutine(InitUnScaldTimeForBeta());
		}

		isActive = false;
		unScaldTime = 0.0f;
		
		alphaMat.SetFloat("_UnTimeScale", unScaldTime);
		betaMat.SetFloat("_UnTimeScale", unScaldTime);
	}
	
	private IEnumerator InitUnScaldTimeForAlpha()
	{
		while (isActive == true)
		{
			unScaldTime += Time.unscaledDeltaTime * delayTime;
			alphaMat.SetFloat("_UnTimeScale", unScaldTime);

			yield return null;
		}
	}

	private IEnumerator InitUnScaldTimeForBeta()
	{
		while (isActive == true)
		{
			unScaldTime += Time.unscaledDeltaTime * delayTime;

			alphaMat.SetFloat("_UnTimeScale", unScaldTime);
			betaMat.SetFloat("_UnTimeScale", unScaldTime);

			yield return null;
		}
	}

	private void OnDisable()
	{
		if (alphaMat != null)
		{
			alphaMat.SetFloat("_UnTimeScale", 0);
		}

		if (betaMat != null)
		{
			betaMat.SetFloat("_UnTimeScale", 0);
		}
	}
}
