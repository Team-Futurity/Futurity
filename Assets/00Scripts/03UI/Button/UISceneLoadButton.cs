using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLoadButton : UIButton
{
	public float fadeTime = .0f;

	public bool isLoadingScene = true;

	public LoadingData data;

	public GameObject obj;

	protected override void ActiveFunc()
	{
		UIInputManager.Instance.ClearAll();

		FadeManager.Instance.FadeIn(fadeTime, () =>
		{
			string curChapter = ChapterMoveController.Instance.GetCurrentChapter();
			
			AudioManager.Instance.StopBackgroundMusic();
			SceneLoader.Instance.data = data;
			SceneLoader.Instance.LoadScene(curChapter, isLoadingScene);
			InputActionManager.Instance.DisableActionMap();
			UIInputManager.Instance.ClearAll();
		});
	}

	public override void SelectActive(bool isOn)
	{
		if(obj != null)
			obj.SetActive(isOn);
	}
}
