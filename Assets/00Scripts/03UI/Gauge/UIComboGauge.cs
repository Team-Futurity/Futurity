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
			FDebug.Log($"[{GetType()}] 필요한 Instance가 존재하지 않습니다.");
			FDebug.Break();

			return;
		}

		ComboSystem.OnGaugeChanged?.AddListener((current, max) =>
		{
			UpdateComboGauge(current, max);
		});
	}

	public void SetDefault()
	{
		Gauge.SetFill(ComboSystem.CurrentGauge, ComboSystem.maxComboGauge);
	}
	
	private void UpdateComboGauge(float currentGauge, float maxGauge)
	{
		if(currentGauge > maxGauge)
		{
			currentGauge = maxGauge;
		}
		if(Gauge.gameObject.activeSelf)
			Gauge.StartFillGauge(currentGauge, maxGauge);
	}
}
