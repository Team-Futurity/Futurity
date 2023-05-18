using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBarController : MonoBehaviour
{
	[SerializeField]
	private Image comboGaugeBar;

	[SerializeField]
	private float gaugeBarfillAmount = 1;
	[SerializeField]
	private int gaugeIntegerPercent = 100;


	public void SetGaugeFillAmount(float setValue)
	{
		gaugeBarfillAmount = setValue;
		gaugeIntegerPercent = (int)(gaugeBarfillAmount * 100);
		comboGaugeBar.fillAmount = gaugeBarfillAmount;
	}
	public float GetGaugeFillAmount()
	{
		return gaugeBarfillAmount;
	}
	public int GetGaugeIntegerPercent()
	{
		return gaugeIntegerPercent;
	}
}
