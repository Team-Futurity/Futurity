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

		// �� ��� �̵� ����
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
