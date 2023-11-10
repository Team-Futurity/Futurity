using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHPGauge : MonoBehaviour
{
	[field: SerializeField]
	public UIGauge Gauge { get; private set; }

	[field: SerializeField]
	public UnitBase playerUnitBase { get; private set; }

	public Image realImage;

	[SerializeField]
	private bool isBillboard = false;

	private Camera mainCam;
	
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
			realImage.fillAmount = current / max;
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
