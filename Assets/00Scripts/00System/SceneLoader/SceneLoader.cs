using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SceneLoader : Singleton<SceneLoader>
{
	public float sceneProgress = .0f;

	private const string loadSceneName = "LoadingScene";

	private string nextSceneName = "";

	public LoadingData data;

	protected override void Awake()
	{
		base.Awake();
	}

	private void EnableSceneLoadEvent()
	{
		SceneManager.sceneLoaded += SetLoadSystemData;
	}

	private void DisableSceneLoadEvent()
	{
		SceneManager.sceneLoaded -= SetLoadSystemData;
	}

	private void SetLoadSystemData(Scene scene, LoadSceneMode mode)
	{
		var loadSystemObject = GameObject.Find("Loading System Group");

		if (loadSystemObject == null)
		{
			return;
		}

		loadSystemObject.TryGetComponent<LoadingSystem>(out var loadSystem);

		data = Addressables.LoadAssetAsync<LoadingData>(nextSceneName).WaitForCompletion();

		if(data != null) loadSystem.SetLoadData(data);
		loadSystem.SetNextScene(nextSceneName);

		DisableSceneLoadEvent();
	}

	public void LoadScene(string sceneName)
	{
		nextSceneName = sceneName;

		EnableSceneLoadEvent();
		SceneManager.LoadScene(loadSceneName);
	}

	public void LoadScene(string sceneName, bool usedLoadScene = true, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		nextSceneName = sceneName;

		SceneManager.LoadScene( 
			(usedLoadScene == true) ? loadSceneName : nextSceneName, 
			mode
			);

		if (usedLoadScene){ EnableSceneLoadEvent();}

		endAction?.Invoke();
	}

	public void LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		StartCoroutine(StartAsyncSceneLoad(sceneName, mode, endAction));
	}

	public UnityEvent<float> updateProgress;

	private IEnumerator StartAsyncSceneLoad(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, UnityAction endAction = null)
	{
		var operation = SceneManager.LoadSceneAsync(sceneName, mode);

		operation.allowSceneActivation = false;

		var timer = .0f;
		sceneProgress = .0f;

		while (!operation.isDone)
		{
			timer += 0.1f;

			sceneProgress = operation.progress / 0.9f;
			updateProgress?.Invoke(sceneProgress);

			yield return new WaitForSeconds(0.1f);

			if (sceneProgress > 0.95f && timer >= 1f)
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

