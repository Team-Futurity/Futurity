using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{
	public bool isEasing = false;

	[Space(10), Range(0.001f, 0.01f)]
	public float timeWeight = .0f;

	private Image gaugeImage;

	private float currentGauge = .0f;

	private float targetGauge = .0f;
	private float maxGauge = .0f;

	private bool isFilling = false;

	private float timer = .0f;
	private float activeTime = .0f;


	private void Awake()
	{
		TryGetComponent(out gaugeImage);
	}

	public void SetGauge(float currentGauge, float maxGauge)
	{
		gaugeImage.fillAmount = currentGauge / maxGauge;
	}

	public void StartFillGauge(float targetGaugeValue, float maxGaugeValue)
	{
		if (isFilling)
		{
			StopCoroutine("FillGauge");
		}

		isFilling = true;

		currentGauge = gaugeImage.fillAmount * maxGaugeValue;

		activeTime += Mathf.Abs(targetGaugeValue - currentGauge) * timeWeight;

		targetGauge = targetGaugeValue;
		maxGauge = maxGaugeValue;

		StartCoroutine("FillGauge");
	}

	private IEnumerator FillGauge()
	{
		while (timer <= activeTime)
		{
			var resultGauge = isEasing ? EaseOutExpo(currentGauge, targetGauge, timer / activeTime) : Mathf.Lerp(currentGauge, targetGauge, timer / activeTime);

			gaugeImage.fillAmount = resultGauge;

			yield return new WaitForSeconds(0.01f);

			timer += Time.deltaTime;
		}

		activeTime = .0f;
		timer = .0f;

		isFilling = false;
	}

	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
	}
}