using UnityEngine;
using System.Collections;

public class ShakeEffect : MonoBehaviour
{
	public float duration = 0.5f;
	public float magnitude = 0.1f;

	public AnimationCurve shakeCurve;

	private Vector3 originalPosition;
	private bool isShaking = false;
	private Coroutine currentShakeCoroutine;

	void Awake()
	{
		originalPosition = transform.localPosition;
	}

	public void StartShake()
	{
		if (isShaking)
		{
			StopCoroutine(currentShakeCoroutine);
		}

		currentShakeCoroutine = StartCoroutine(Shake());
	}

	IEnumerator Shake()
	{
		isShaking = true;
		float elapsed = 0.0f;

		while (elapsed < duration)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;

			// Use the curve to get the current shake magnitude
			float currentMagnitude = shakeCurve.Evaluate(elapsed / duration) * magnitude;

			transform.localPosition = originalPosition + new Vector3(x, y, 0) * currentMagnitude;

			elapsed += Time.deltaTime;

			yield return null;
		}

		transform.localPosition = originalPosition;
		isShaking = false;
	}
}
