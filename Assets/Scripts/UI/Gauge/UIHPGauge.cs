using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHPGauge : MonoBehaviour
{
	[field: SerializeField]
	public UIGauge Gauge { get; private set; }

	[field: SerializeField]
	public StatusManager StatusManager { get; private set; }

	private void Awake()
	{
		if (Gauge == null || StatusManager == null)
		{
			FDebug.Log($"[{GetType()}] 필요한 Instance가 존재하지 않습니다.");
			FDebug.Break();

			return;
		}
	}

	private void Start()
	{
		StatusManager.GetStatus(StatusType.CURRENT_HP).UpdateStatus.AddListener( (x) => {
			UpdateHPGauge(x);
		});
	}

	private void UpdateHPGauge(float gauge)
	{
		var maxHpElement = StatusManager.GetStatus(StatusType.MAX_HP).GetValue();

		if (gauge > maxHpElement)
		{
			gauge = maxHpElement;
		}

		Gauge.StartFillGauge(gauge);
	}
}
