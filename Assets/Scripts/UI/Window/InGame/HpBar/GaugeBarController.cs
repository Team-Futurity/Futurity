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

	[SerializeField]
	float gagueDuration = 1f;

	private Coroutine fillRoutine;

	/// <summary>
	/// 게이지바의 채워질 값을 설정하고 코루틴을 시작합니다.
	/// </summary>
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

	/// <summary>
	/// 게이지바를 지정된 값까지 시간에 따라 부드럽게 채웁니다.
	/// </summary>
	private IEnumerator FillGaugeOverTime(float target)
	{
		float time = 0f;
		float startValue = gaugeBar.fillAmount;

		while (time < gagueDuration)
		{
			float easedTime = EaseInOut(time / gagueDuration);
			gaugeBar.fillAmount = Mathf.Lerp(startValue, target, easedTime);
			time += Time.deltaTime;
			yield return null;
		}

		gaugeBar.fillAmount = target;
	}

	/// <summary>
	/// EaseInOut(보간) 함수로, 입력 값에 대해 부드러운 이동을 위한 값을 반환합니다.
	/// </summary>
	private float EaseInOut(float t)
	{
		return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
	}

	/// <summary>
	/// 게이지 바의 현재 채움량을 반환합니다.
	/// </summary>
	public float GetGaugeFillAmount()
	{
		return gaugeBarfillAmount;
	}

	/// <summary>
	/// 게이지 바의 채움량을 퍼센트로 나타낸 정수 값을 반환합니다.
	/// </summary>
	public int GetGaugeIntegerPercent()
	{
		return gaugeIntegerPercent;
	}
}