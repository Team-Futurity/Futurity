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

	[SerializeField]
	private bool isBillboard = false;

	private Camera mainCam;


	private void Awake()
	{
		if (Gauge == null || playerUnitBase == null)
		{
			FDebug.Log($"[{GetType()}] 필요한 Instance가 존재하지 않습니다.");
			FDebug.Break();

			return;
		}

		playerUnitBase.status.updateHPEvent?.AddListener((current, max) =>
		{
			UpdateHPGauge(current,max);
		});

		mainCam = Camera.main;
	}

	private void LateUpdate()
	{
		if(!isBillboard)
		{
			return;
		}

		transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward, mainCam.transform.rotation * Vector3.up);
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
