using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComboGauge : MonoBehaviour
{
	[field: SerializeField]
	public UIGauge Gauge { get; private set; }

	[field: SerializeField]
	public ComboGaugeSystem ComboSystem { get; private set; }

	private void Awake()
	{
		if (Gauge == null || ComboSystem == null)
		{
			FDebug.Log($"[{GetType()}] �ʿ��� Instance�� �������� �ʽ��ϴ�.");
			FDebug.Break();

			return;
		}

		ComboSystem.OnGaugeChanged?.AddListener((current, max) =>
		{
			UpdateComboGauge(current, max);
		});
	}

	private void UpdateComboGauge(float currentGauge, float maxGauge)
	{
		if(currentGauge > maxGauge)
		{
			currentGauge = maxGauge;
		}

		Gauge.StartFillGauge(currentGauge, maxGauge);
	}
}
