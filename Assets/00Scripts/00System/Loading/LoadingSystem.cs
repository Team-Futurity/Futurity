using FMODUnity;
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

	private string nextScene = "";
	private void Update()
	{
		if (!isFillGaugeStart)
			return;

		FillLoadingGauge(SceneLoader.Instance.sceneProgress);
	}

	public void SetNextScene(string sceneName)
	{
		nextScene = sceneName;

		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			AudioManager.Instance.CleanUp();

			SceneLoader.Instance.LoadSceneAsync(nextScene);
			isFillGaugeStart = true;
		});
	}

	private void FillLoadingGauge(float targetValue)
	{
		loadingGauge.StartFillGauge(targetValue * 100f, 1f);
	}
}
