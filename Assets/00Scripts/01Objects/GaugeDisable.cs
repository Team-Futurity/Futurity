using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeDisable : MonoBehaviour
{
	public UIHPGauge playerGauge;
	public UIComboGauge comboGauge;
	public UIHPGauge bossGauge;
	
	public void SetGauge(bool isOn)
	{
		if (!isOn)
		{
			if (playerGauge != null)
				playerGauge.Gauge.StopGauge();
			if (comboGauge != null)
				comboGauge.Gauge.StopGauge();
			if (bossGauge != null)
				bossGauge.Gauge.StopGauge();
		}
		else
		{
			if(playerGauge != null)
				playerGauge.SetDefault();
			if(comboGauge != null)
				comboGauge.SetDefault();
			if(bossGauge != null)
				bossGauge.Gauge.StopGauge();
		}
	}
}
