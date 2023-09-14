using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : Singleton<SceneLoader>
{
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		SceneManager.LoadScene(sceneName, mode);

		endAction?.Invoke();
	}

	public void LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		var oper = SceneManager.LoadSceneAsync(sceneName, mode);

		endAction?.Invoke();
	}
}
