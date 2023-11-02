using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingSystem : MonoBehaviour
{
	[SerializeField, Header("씬 Fade 시간")]
	private float fadeTime = 1f;

	[SerializeField, Header("로딩 아이콘")]
	private LoadingIconMove loadIcon;

	private string nextScene = "";

	public Image img;
	public TMP_Text cctvText;
	public TMP_Text newsText;


	public void SetLoadData(LoadingData data)
	{
		if (data == null) return;

		img.sprite = data.cctveImage;
		cctvText.text = data.cctvText;
		newsText.text = data.newsText;
	}

	public void SetNextScene(string sceneName)
	{
		nextScene = sceneName;

		UIManager.Instance.RemoveAllWindow();

		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			AudioManager.Instance.CleanUp();

			SceneLoader.Instance.updateProgress?.AddListener(loadIcon.MoveIcon);
			SceneLoader.Instance.LoadSceneAsync(nextScene);
		});
	}
}
