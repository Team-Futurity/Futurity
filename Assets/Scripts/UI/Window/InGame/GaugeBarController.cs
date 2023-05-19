using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBarController : MonoBehaviour
{
	[SerializeField]
	public Image gaugeBar;
	private Image comboGaugeTempBar;

	[SerializeField]
	private float gaugeBarfillAmount = 1;
	[SerializeField]
	private int gaugeIntegerPercent = 100;

	private Coroutine fillRoutine;

	public void SetGaugeFillAmount(float setValue)
	{
		gaugeBarfillAmount = setValue;
		gaugeIntegerPercent = (int)(gaugeBarfillAmount * 100);

		if (fillRoutine != null)
		{
			StopCoroutine(fillRoutine);
		}

		fillRoutine = StartCoroutine(FillGaugeOverTime(gaugeBarfillAmount));
	}

	private IEnumerator FillGaugeOverTime(float target)
	{
		float time = 0f;
		float startValue = gaugeBar.fillAmount;
		float duration = 1f;

		while (time < duration)
		{
			float easedTime = EaseInOut(time / duration);
			gaugeBar.fillAmount = Mathf.Lerp(startValue, target, easedTime);
			time += Time.deltaTime;
			yield return null;
		}

		gaugeBar.fillAmount = target;
	}

	private float EaseInOut(float t)
	{
		return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
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