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
	/// ���������� ä���� ���� �����ϰ� �ڷ�ƾ�� �����մϴ�.
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
	/// �������ٸ� ������ ������ �ð��� ���� �ε巴�� ä��ϴ�.
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
	/// EaseInOut(����) �Լ���, �Է� ���� ���� �ε巯�� �̵��� ���� ���� ��ȯ�մϴ�.
	/// </summary>
	private float EaseInOut(float t)
	{
		return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
	}

	/// <summary>
	/// ������ ���� ���� ä���� ��ȯ�մϴ�.
	/// </summary>
	public float GetGaugeFillAmount()
	{
		return gaugeBarfillAmount;
	}

	/// <summary>
	/// ������ ���� ä���� �ۼ�Ʈ�� ��Ÿ�� ���� ���� ��ȯ�մϴ�.
	/// </summary>
	public int GetGaugeIntegerPercent()
	{
		return gaugeIntegerPercent;
	}
}