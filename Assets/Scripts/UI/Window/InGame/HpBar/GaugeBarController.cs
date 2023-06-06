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
	/// ���������� ä���� ���� �����ϰ� �ڷ�ƾ�� �����ϴ� �޼����Դϴ�.
	/// </summary>
	/// <param name="setValue">������ ������ �� ����(0~1)</param>
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
	/// �������ٸ� ������ ������ �ð��� ���� �ε巴�� ä��ϴ�.
	/// </summary>
	/// <param name="target">������ �� ����</param>
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
	/// ���� �Լ���, �Է� ���� ���� �ε巯�� �̵��� ���� ���� ��ȯ�մϴ�.
	/// </summary>
	private float EaseInOut(float t)
	{
		return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
	}

	/// <summary>
	/// ���������� ���� ä���� ������ ��ȯ�ϴ� �޼����Դϴ�.
	/// </summary>
	/// <returns>���� ������ ���� ����</returns>
	public float GetGaugeFillAmount()
	{
		return gaugeBarfillAmount;
	}

	/// <summary>
	/// ���������� ���� ä���� ������ �ۼ�Ʈ�� ��ȯ�� ���� ��ȯ�ϴ� �޼����Դϴ�.
	/// </summary>
	/// <returns>���� ������ ���� �ۼ�Ʈ</returns>
	public int GetGaugeIntegerPercent()
	{
		return gaugeIntegerPercent;
	}
}