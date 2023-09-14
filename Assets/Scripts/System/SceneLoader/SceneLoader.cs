using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : Singleton<SceneLoader>
{
	public float sceneProgress = .0f;

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
		StartCoroutine(StartAsyncSceneLoad(sceneName, mode, endAction));
	}

	private IEnumerator StartAsyncSceneLoad(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		var operation = SceneManager.LoadSceneAsync(sceneName, mode);

		// 씬 즉시 이동 해제
		operation.allowSceneActivation = false;

		while (!operation.isDone)
		{
			sceneProgress = operation.progress;

			yield return null;
		}

		endAction?.Invoke();
	}
}
