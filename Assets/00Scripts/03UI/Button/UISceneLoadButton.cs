using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLoadButton : UIButton
{
	public float fadeTime = .0f;

	public bool isLoadingScene = true;
	public string loadSceneName = "";

	public LoadingData data;

	public GameObject obj;

	protected override void ActiveFunc()
	{
		UIInputManager.Instance.ClearAll();
		Time.timeScale = 1f;

		FadeManager.Instance.FadeIn(fadeTime, () =>
		{
			if (string.Equals(loadSceneName, "") == false)
			{
				SceneLoader.Instance.data = data;
				SceneLoader.Instance.LoadScene(loadSceneName, isLoadingScene);
				InputActionManager.Instance.DisableActionMap();
				UIInputManager.Instance.ClearAll();
			}
			else
			{
				string curChapter = ChapterMoveController.Instance.GetCurrentChapter();
				if (string.Equals(curChapter, ChapterSceneName.CHAPTER1_1) == true)
				{
					PlayerPrefs.SetInt("Chapter1", 1);
				}
			
				AudioManager.Instance.StopBackgroundMusic();
				SceneLoader.Instance.data = data;
				SceneLoader.Instance.LoadScene(curChapter, isLoadingScene);
				InputActionManager.Instance.DisableActionMap();
				UIInputManager.Instance.ClearAll();	
			}
		});
	}

	public override void SelectActive(bool isOn)
	{
		if(obj != null)
			obj.SetActive(isOn);
	}
}
