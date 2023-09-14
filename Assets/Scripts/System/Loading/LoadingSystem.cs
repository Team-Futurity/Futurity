using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
	[field: SerializeField]
	private UIGauge loadingGauge;

	private bool isActionStart = false;

	private void Start()
	{
		Debug.Log("START LOADING PROCESS");

		loadingGauge.SetCurrentGauge(0f);

		Debug.Log("START COROUTINE - FADE OUT");
		FadeManager.Instance.FadeOut(0.3f, () =>
		{
			SceneLoader.Instance.LoadSceneAsync("TutorialScene");

			isActionStart = true;
		});
	}

	private void Update()
	{
		if (isActionStart)
		{
			FillLoadingGauge(SceneLoader.Instance.sceneProgress);
		}
	}

	private void FillLoadingGauge(float targetValue)
	{
		loadingGauge.StartFillGauge(targetValue * 100f, 3f);
	}
}
