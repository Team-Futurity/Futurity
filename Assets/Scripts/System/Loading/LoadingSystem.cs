using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
	[field: SerializeField]
	private UIGauge loadingGauge;

	private void Start()
	{
		loadingGauge.SetCurrentGauge(0f);

		FadeManager.Instance.FadeOut(1f, () =>
		{
			SceneLoader.Instance.LoadSceneAsync("TutorialScene",
												UnityEngine.SceneManagement.LoadSceneMode.Single,
												() =>
												{
													// Scene Load가 끝나고 취할 행동
													// Fade In -> 
												});

			FillLoadingGauge(SceneLoader.Instance.sceneProgress);
		});
	}

	private void FillLoadingGauge(float targetValue)
	{
		loadingGauge.StartFillGauge(targetValue * 100f);
	}
}
