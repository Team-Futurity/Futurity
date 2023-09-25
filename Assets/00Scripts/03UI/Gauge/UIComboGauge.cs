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
	}

	private void Start()
	{
		ComboSystem.OnGaugeChanged?.AddListener(UpdateComboGauge);
	}

	private void UpdateComboGauge(float gauge)
	{
		if(gauge > 100f)
		{
			gauge = 100f;
		}

		// Gauge.StartFillGauge(gauge);
	}
}
