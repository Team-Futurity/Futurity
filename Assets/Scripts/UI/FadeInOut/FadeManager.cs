using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
	[field: SerializeField] public Image FadeImage { get; private set; }

	private readonly float MAX = 1f;
	private readonly float MIN = 0f;

	private float startAlpha = .0f;
	private float targetAlpha = .0f;

	private float timer;
	private bool isFading;

	protected override void Awake()
	{
		FadeImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
	}

	public void FadeIn(float time = 0f, UnityAction inAction = null)
	{
		if (isFading)
		{
			return;
		}

		SetFadeRun(true);

		StartCoroutine(UpdateScreenFade(MIN, MAX, time, inAction));
	}

	public void FadeOut(float time = 0f, UnityAction outAction = null)
	{
		if (isFading)
		{
			return;
		}

		SetFadeRun(true);

		StartCoroutine(UpdateScreenFade(MAX, MIN, time, outAction));
	}

	private IEnumerator UpdateScreenFade(float start, float end, float time, UnityAction updateAction = null)
	{
		var imageColor = FadeImage.color;

		while (timer < time)
		{
			timer += Time.deltaTime;

			imageColor.a = Mathf.Lerp(start, end, timer / time);
			FadeImage.color = imageColor;

			yield return null;
		}

		// is Fading OFF
		SetFadeRun(false);
		timer = .0f;

		// Used Action
		updateAction?.Invoke();
	}

	private void SetFadeRun(bool isTrigger)
	{
		isFading = isTrigger;
		FadeImage.raycastTarget = !isTrigger;
	}
}