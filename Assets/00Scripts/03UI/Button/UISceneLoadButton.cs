using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLoadButton : UIButton
{
	public float fadeTime = .0f;
	public string sceneName = "";

	public bool isLoadingScene = true;

	public LoadingData data;

	protected override void ActiveFunc()
	{
		UIInputManager.Instance.ClearAll();

		FadeManager.Instance.FadeIn(fadeTime, () =>
		{
			SceneLoader.Instance.LoadScene(sceneName, isLoadingScene);
			SceneLoader.Instance.data = data;
			InputActionManager.Instance.DisableActionMap();
			UIInputManager.Instance.ClearAll();
		});
	}
}
