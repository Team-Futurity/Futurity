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

	/// <summary>
	/// 게이지바의 채워질 값을 설정하고 코루틴을 시작하는 메서드입니다.
	/// </summary>
	/// <param name="setValue">변경할 게이지 바 비율(0~1)</param>
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
	/// <param name="target">게이지 바 비율</param>
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

	/// <summary>
	/// 보간 함수로, 입력 값에 대해 부드러운 이동을 위한 값을 반환합니다.
	/// </summary>
	private float EaseInOut(float t)
	{
		return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
	}

	/// <summary>
	/// 게이지바의 현재 채워진 비율을 반환하는 메서드입니다.
	/// </summary>
	/// <returns>현재 게이지 바의 비율</returns>
	public float GetGaugeFillAmount()
	{
		return gaugeBarfillAmount;
	}

	/// <summary>
	/// 게이지바의 현재 채워진 비율을 퍼센트로 변환한 값을 반환하는 메서드입니다.
	/// </summary>
	/// <returns>현재 게이지 바의 퍼센트</returns>
	public int GetGaugeIntegerPercent()
	{
		return gaugeIntegerPercent;
	}
}