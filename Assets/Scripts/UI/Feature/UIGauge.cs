using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{
	[field: SerializeField] public AnimationCurve FillCurve { get; private set; }

	private Image gaugeImage;

	private float currentGaugeValue = .0f;
	private float targetGauge = .0f;
	
	public float timer = .0f;
		
	private void Awake()
	{
		TryGetComponent(out gaugeImage);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			StartCoroutine(FillGauge(targetGauge));
		}
	}

	// Gauge�� targetValue���� ä���.
	private IEnumerator FillGauge(float target)
	{
		float startGaugeValue = currentGaugeValue;
		float targetGaugeValue = target;

		while (timer >= 1f)
		{
			gaugeImage.fillAmount = Mathf.Lerp(startGaugeValue, targetGaugeValue, timer);
			
			yield return null;

			timer += 0.1f;
		}

		currentGaugeValue = targetGaugeValue;

		Debug.Log("��ǥġ �Ϸ�");
	}
}