using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeManager : Singleton<FadeManager>
{
	[SerializeField]
	private Image fadeImage;

	private readonly float MAX = 1f;
	private readonly float MIN = 0f;

	private float startAlpha = .0f;
	private float targetAlpha = .0f;

	private float timer = .0f;
	[HideInInspector]
	public bool isFading;
	

	protected override void Awake()
	{
		base.Awake();

		if (fadeImage == null)
		{
			TryGetComponent(out fadeImage);
		}
	}

	public void FadeIn(float time = 1f, UnityAction inAction = null)
	{
		if (isFading)
		{
			return;
		}

		SetFadeRun(true);
		SetFadeImage(MIN);
		fadeImage.enabled = true;

		StartCoroutine(UpdateScreenFade(MIN, MAX, time, inAction));
	}

	public void FadeOut(float time = 1f, UnityAction outAction = null)
	{
		if (isFading)
		{
			return;
		}

		SetFadeRun(true);
		SetFadeImage(MAX);

		StartCoroutine(UpdateScreenFade(MAX, MIN, time, outAction));
	}

	private IEnumerator UpdateScreenFade(float start, float end, float time, UnityAction endAction = null)
	{
		var imageColor = fadeImage.color;


		while (timer <= time)
		{
			imageColor.a = Mathf.Lerp(start, end, timer / time);
			fadeImage.color = imageColor;

			yield return null;

			timer += Time.deltaTime;
		}

		// is Fading OFF
		SetFadeRun(false);
		timer = .0f;

		// Used Action
		endAction?.Invoke();
	}

	private void SetFadeRun(bool isTrigger)
	{
		isFading = isTrigger;
		fadeImage.raycastTarget = !isTrigger;
	}

	private void SetFadeImage(float ratio)
	{
		var imageColor = fadeImage.color;
		imageColor.a = ratio;
		
		fadeImage.color = imageColor;
	}
}