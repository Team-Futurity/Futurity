using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
	[SerializeField]
	private UIGauge loadingGauge;

	[SerializeField]
	private float fadeTime = 1f;

	private bool isFillGaugeStart = false;

	private void Start()
	{
		loadingGauge.SetCurrentGauge(0f);

		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			SceneLoader.Instance.LoadSceneAsync("TutorialScene");
			isFillGaugeStart = true;
		});
	}

	private void Update()
	{
		if (!isFillGaugeStart)
			return;

		FillLoadingGauge(SceneLoader.Instance.sceneProgress);
	}

	private void FillLoadingGauge(float targetValue)
	{
		loadingGauge.StartFillGauge(targetValue * 100f, 3f);
	}
}
