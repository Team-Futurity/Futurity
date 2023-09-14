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
		maxTIme = max;

		StartCoroutine(FillGauge(target));
	}

	// Gauge�� targetValue���� ä���.
	private IEnumerator FillGauge(float target)
	{
		float startGaugeValue = currentGaugeValue;
		float targetGaugeValue = target;

		// N�� ���� �ٷ� ���� �ϱ�
		while (timer <= maxTIme)
		{
			var resultEasing = EaseOutExpo(startGaugeValue, targetGaugeValue, timer / maxTIme);
			
			// 0 ~ 1f
			gaugeImage.fillAmount = resultEasing / targetGaugeValue;

			yield return null;

			timer += Time.deltaTime;
		}

		timer = .0f;

		currentGaugeValue = targetGaugeValue;

		Debug.Log("��ǥġ �Ϸ�");
	}

	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
	}

}