using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChangerController
{
	public static void SceneChange(SceneKeyData sceneKeyData, int loadingType)
	{
		SceneChangeManager.Instance.SceneLoad(sceneKeyData, loadingType);
	}

	public static void ResetScene()
	{
		SceneChangeManager.Instance.SelfSceneLoad();
	}
}
