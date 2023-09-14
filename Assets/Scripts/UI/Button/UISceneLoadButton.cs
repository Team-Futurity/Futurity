using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneLoadButton : UIButton
{
	public float fadeTime = .0f;

	protected override void ActiveAction()
	{
		SceneLoader.Instance.LoadScene("LoadingScene 1");

		//FadeManager.Instance.FadeIn(fadeTime, () =>
		//{
		//});
	}
}
