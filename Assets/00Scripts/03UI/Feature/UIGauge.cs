using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{
	public bool isEasing = false;

	private Image gaugeImage;

	[Range(0.01f, 2f), SerializeField]
	private float gaugeOffset = 0.5f;

	private float currentGauge = .0f;
	private float targetGauge = .0f;
	private float maxGauge = .0f;

	private bool isFilling = false;

	private float timer = .0f;
	private float activeTime = .0f;
	
	private float progressGauge = .0f;

	private WaitForSeconds gaugeFillTime = new WaitForSeconds(0.01f);

	private void Awake()
	{
		TryGetComponent(out gaugeImage);
	}

	public void StartFillGauge(float targetGaugeValue, float maxGaugeValue)
	{
		if (!gameObject.activeSelf)
		{
			return;
		}
		
		if (isFilling)
		{
			StopCoroutine("FillGauge");
			currentGauge = progressGauge;
		}
		
		isFilling = true;

		targetGauge = targetGaugeValue;
		maxGauge = maxGaugeValue;

		activeTime += Mathf.Abs((targetGauge / maxGauge) - (currentGauge / maxGauge)) * gaugeOffset;
		
		if (activeTime <= 0f)
		{
			return;
		}
		
		StartCoroutine("FillGauge");
	}

	public void StartLoadingGauge(float targetGaugeValue, float maxGaugeValue)
	{
		targetGauge = targetGaugeValue;
		maxGauge = maxGaugeValue;
		activeTime = 3f;

		StartCoroutine(FillLoadGauge());
	}

	public void SetFill(float target, float max)
	{
		gaugeImage.fillAmount = target / max;
	}

	private IEnumerator FillLoadGauge()
	{
		while (currentGauge <= targetGauge)
		{
			timer += Time.deltaTime;
			
			yield return gaugeFillTime;

			progressGauge = Mathf.Lerp(currentGauge, targetGauge, timer / activeTime);

			gaugeImage.fillAmount = progressGauge / maxGauge;
		}
	}

	private IEnumerator FillGauge()
	{
		while (timer <= activeTime)
		{
			timer += Time.deltaTime;

			yield return gaugeFillTime;

			progressGauge = isEasing ? 
				EaseOutExpo(currentGauge, targetGauge, timer / activeTime) 
				: Mathf.Lerp(currentGauge, targetGauge, timer / activeTime);
			
			gaugeImage.fillAmount = progressGauge / maxGauge;
		}
		
		activeTime = .0f;
		timer = .0f;
		currentGauge = targetGauge;

		isFilling = false;
	}

	private float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
	}
}