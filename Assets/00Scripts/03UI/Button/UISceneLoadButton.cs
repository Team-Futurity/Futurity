using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLoadButton : UIButton
{
	public float fadeTime = .0f;
	public string sceneName = "";

	protected override void ActiveFunc()
	{
		UIInputManager.Instance.ClearAll();
		InputActionManager.Instance.DisableAllInputActionAsset();

		FadeManager.Instance.FadeIn(fadeTime, () =>
		{
			SceneLoader.Instance.LoadScene(sceneName);
		});
	}
}
