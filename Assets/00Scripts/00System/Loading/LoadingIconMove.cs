using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIconMove : MonoBehaviour
{
	public RectTransform icon;
	private Slider slider;

	private Vector2 startPos;
	private Vector2 endPos;

	private bool isActive = false;

	private float timer = .0f;

	private void Awake()
	{
		slider = GetComponent<Slider>();
	}

	public void MoveIcon(float percent)
	{
		if (percent <= .0f || isActive) return;

		if (percent <= 0.3f) return;

		isActive = true;

		timer = percent;

		StartCoroutine("StartMove");
	}

	private IEnumerator StartMove()
	{
		while(slider.value <= 1.0f)
		{
			timer += Time.deltaTime;

			yield return new WaitForSeconds(0.01f);

			slider.value = Mathf.Lerp(0, 1, timer / 1.5f);
		}
	}
}
