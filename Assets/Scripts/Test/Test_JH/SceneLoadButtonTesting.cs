using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadButtonTesting : MonoBehaviour
{
    public string loadSceneName;
	private void Awake()
	{
		var _ = Singleton.Instance;
	}
	public void Click()
	{

		if (Singleton.Instance == null)
		{
			Debug.LogError("Singleton.Instance is null");
			return;
		}
		if (Singleton.Instance.sceneChanger == null)
		{
			Debug.LogError("Singleton.Instance.sceneChanger is null");
			return;
		}
		Singleton.Instance.sceneChanger.SceneLoader(loadSceneName);
    }
}
