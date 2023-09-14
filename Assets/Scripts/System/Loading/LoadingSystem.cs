using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
	[field: SerializeField]
	private UIGauge loadingGauge;

	private void Awake()
	{
		loadingGauge.SetCurrentGauge(0f);
	}

	private void FillLoadingGauge(float targetValue)
	{
		// Target Value : Progress Gauge
		loadingGauge.StartFillGauge(3f);
	}
}
