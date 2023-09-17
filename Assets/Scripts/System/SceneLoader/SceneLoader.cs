using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : Singleton<SceneLoader>
{
	public float sceneProgress = .0f;


	// Only Title
	private readonly string LoadingScene1 = "LoadingScene 1";

	private readonly string LoadingScene2 = "LoadingScene 2";

	private string nextSceneName = "";

	protected override void Awake()
	{
		base.Awake();

		SceneManager.sceneLoaded += Test;
	}

	private void Test(Scene scene, LoadSceneMode mode)
	{
		GameObject.Find("Loading System Group").TryGetComponent<LoadingSystem>(out var loadSystem);
		loadSystem.SetNextScene(nextSceneName);
	}

	public void LoadScene(string sceneName)
	{
		nextSceneName = sceneName;

		SceneManager.LoadScene((sceneName == "TutorialScene") ? LoadingScene1 : LoadingScene2);
	}

	public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		nextSceneName = sceneName;

		SceneManager.LoadScene((sceneName == "TutorialScene") ? LoadingScene1 : LoadingScene2, mode);

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

			// SceneProgress가 95% 이상 Load되고, Time이 2초 이상 지났을 경우 Scene Load
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
