using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public string loadSceneName;

	public void SceneChange()
	{
		SceneChangeManager.Instance.SceneLoader(loadSceneName);
    }
}
