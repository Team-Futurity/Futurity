using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSystem : MonoBehaviour
{
	[SerializeField, Header("�� Fade �ð�")]
	private float fadeTime = 1f;

	[SerializeField, Header("�ε� ������")]
	private LoadingIconMove loadIcon;

	private string nextScene = "";

	public void SetNextScene(string sceneName)
	{
		nextScene = sceneName;

		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			AudioManager.Instance.CleanUp();

			SceneLoader.Instance.updateProgress?.AddListener(loadIcon.MoveIcon);
			SceneLoader.Instance.LoadSceneAsync(nextScene);
		});
	}
}
