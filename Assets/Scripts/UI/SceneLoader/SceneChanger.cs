using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	private SceneChangeManager sceneChangeManager;
	private void Start()
	{
		sceneChangeManager = SceneChangeManager.Instance;
	}

	public void SceneChange(SceneKeyData sceneKeyData)
	{
		sceneChangeManager.SceneLoad(sceneKeyData, 1);
	}

	public void TitleSceneChange(SceneKeyData sceneKeyData)
	{
		sceneChangeManager.SceneLoad(sceneKeyData, 2);
	}

	public void ResetScene()
	{
		sceneChangeManager.SelfSceneLoad();
	}
}
