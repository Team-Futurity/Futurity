using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{
	[field: SerializeField] public AnimationCurve FillCurve { get; private set; }

	private Image gaugeImage;

	private float currentGaugeValue = .0f;
	public float targetGauge = .0f;
	
	private float timer = .0f;
	private float maxTIme = .0f;

	private bool isFillig = false;

	private void Awake()
	{
		TryGetComponent(out gaugeImage);
	}
	public void SetCurrentGauge(float gauge)
	{
		currentGaugeValue = gauge;

		gaugeImage.fillAmount = currentGaugeValue / 100f;
	}

	public void StartFillGauge(float target, float max = 1f)
	{
		if(targetGauge >= target)
		{
			return;
		}

		timer = .0f;
		maxTIme = max;
		currentGaugeValue = (gaugeImage.fillAmount * 100f);
		targetGauge = target;

		if(isFillig)
		{
			return;
		}

		StartCoroutine(FillGauge(target));
	}

	private IEnumerator FillGauge(float target)
	{
		isFillig = true;

		while (timer <= maxTIme)
		{
			var resultEasing = EaseOutExpo(currentGaugeValue, targetGauge, timer / maxTIme);
			
			gaugeImage.fillAmount = resultEasing / targetGauge;

			yield return null;

			timer += Time.deltaTime;
		}

		timer = .0f;

		currentGaugeValue = targetGauge;
		isFillig = false;
	}

	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
	}

}