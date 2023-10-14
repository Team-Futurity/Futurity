using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
	[SerializeField, Header("씬 Fade 시간")]
	private float fadeTime = 1f;

	[SerializeField, Header("로딩 아이콘")]
	private LoadingIconMove loadIcon;

	private string nextScene = "";

	public void SetNextScene(string sceneName)
	{
		nextScene = sceneName;

		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			AudioManager.Instance.CleanUp();

			SceneLoader.Instance.updateProgress?.AddListener(loadIcon.MoveIcon);

			// 해당 메서드의 호출 타이밍을 Load Icon이 목적지에 도착했을 경우, 작동해야 함.
			SceneLoader.Instance.LoadSceneAsync(nextScene);
		});
	}
}
