using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGauge : MonoBehaviour
{
	public float gaugeFillTime = .0f;

	[field: SerializeField]
	public AnimationCurve speedCurve { get; private set; }

	private float targetValue = .0f;
	private float currentValue = .0f;

	private float timer = .0f;

	private Image gaugeImage;

	private void Awake()
	{
		TryGetComponent(out gaugeImage);
	}

	public void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			StartCoroutine(FillGauge(targetValue));
		}
	}

	// gauge를 Target Value까지 채운다.

	public void SetValue(float value)
	{

	}

	private IEnumerator FillGauge(float tValue)
	{
		timer = .0f;

		while (timer < gaugeFillTime)
		{
			// Target Value까지 
			yield return new WaitForSeconds(0.1f);

			timer += gaugeFillTime / Time.deltaTime;
		}

		// Fill Ended
	}
}
