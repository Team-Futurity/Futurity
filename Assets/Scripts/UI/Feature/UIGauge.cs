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
		
	private void Awake()
	{
		TryGetComponent(out gaugeImage);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			StartCoroutine(FillGauge(targetGauge));
		}
	}

	// Gauge�� targetValue���� ä���.
	private IEnumerator FillGauge(float target)
	{
		float startGaugeValue = currentGaugeValue;
		float targetGaugeValue = target;

		while (timer <= 1f)
		{
			// targetGaugeValue ��ġ���� �÷��ش�.
			//gaugeImage.fillAmount = currentGaugeValue++;
			var test = EaseOutExpo(currentGaugeValue, targetGaugeValue, timer);
			gaugeImage.fillAmount = test / 100f;

			yield return new WaitForSeconds(0.01f);

			timer += 0.1f;
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