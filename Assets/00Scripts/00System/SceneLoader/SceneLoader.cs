using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : Singleton<SceneLoader>
{
	public float sceneProgress = .0f;

	// Loading Scene
	private const string loadScene = "LoadingScene";

	private string nextSceneName = "";

	protected override void Awake()
	{
		base.Awake();

		SceneManager.sceneLoaded += SetLoadSystemData;
	}

	private void SetLoadSystemData(Scene scene, LoadSceneMode mode)
	{
		var loadSystemObject = GameObject.Find("Loading System Group");

		if (loadSystemObject == null)
		{
			return;
		}

		loadSystemObject.TryGetComponent<LoadingSystem>(out var loadSystem);
		loadSystem.SetNextScene(nextSceneName);
	}

	public void LoadScene(string sceneName)
	{
		nextSceneName = sceneName;

		// SceneManager.LoadScene(() ? LoadingScene1 : LoadingScene2);
	}

	public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		nextSceneName = sceneName;

		// SceneManager.LoadScene(, mode);

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

		var timer = .0f;
		sceneProgress = .0f;

		while (!operation.isDone)
		{
			timer += Time.deltaTime;

			sceneProgress = operation.progress / 0.9f;

			yield return null;

			if (sceneProgress > 0.95f && timer >= 2f)
			{
				FadeManager.Instance.FadeIn(1f, () =>
				{
					operation.allowSceneActivation = true;
				});

				break;
			}

		}

		endAction?.Invoke();
	}
}
