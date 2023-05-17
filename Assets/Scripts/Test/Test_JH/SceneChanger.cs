using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public void SceneChange(SceneKeyData sceneKeyData)
	{
		SceneChangeManager.Instance.SceneLoader(sceneKeyData);
    }

	public void ResetScene()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex);
	}
}
