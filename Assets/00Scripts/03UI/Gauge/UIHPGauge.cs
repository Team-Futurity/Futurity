using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHPGauge : MonoBehaviour
{
	[field: SerializeField]
	public UIGauge Gauge { get; private set; }

	[field: SerializeField]
	public UnitBase playerUnitBase { get; private set; }

	private void Awake()
	{
		if (Gauge == null || playerUnitBase == null)
		{
			FDebug.Log($"[{GetType()}] �ʿ��� Instance�� �������� �ʽ��ϴ�.");
			FDebug.Break();

			return;
		}
		
		playerUnitBase.status.updateHPEvent?.AddListener((current, max) =>
		{
			UpdateHPGauge(current,max);
		});
	}

	private void UpdateHPGauge(float currentGauge, float maxGauge)
	{
		if (currentGauge > maxGauge)
		{
			currentGauge = maxGauge;
		}

		Gauge.StartFillGauge(currentGauge, maxGauge);
	}
}
