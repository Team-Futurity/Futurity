using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TempTestManager : MonoBehaviour
{
	[SerializeField]
	GaugeBarController comboGuageBarController;
	bool isGaugeUpDown = true;

	void Start()
    {
	}


    void Update()
	{
		ComboGuageBarValueChanger();
	}

	void ComboGuageBarValueChanger()
	{
		float gaugeBarValue = comboGuageBarController.GetGaugeFillAmount();

		Debug.Log(gaugeBarValue);

		if (gaugeBarValue >= 1f)
		{
			isGaugeUpDown = false;
		}
		else if (gaugeBarValue <= 0f)
		{
			isGaugeUpDown = true;
		}

		if (isGaugeUpDown)
		{
			comboGuageBarController.SetGaugeFillAmount(gaugeBarValue + 0.05f);
		}
		else
		{
			comboGuageBarController.SetGaugeFillAmount(gaugeBarValue - 0.05f);
		}
		Debug.Log($"GetGaugeIntegerPercent : { comboGuageBarController.GetGaugeIntegerPercent()}");
	}
}
