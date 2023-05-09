using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadButtonTesting : MonoBehaviour
{
    public string loadSceneName;

	public void Click()
	{
		SceneChanger.Instance.SceneLoader(loadSceneName);
    }
}
