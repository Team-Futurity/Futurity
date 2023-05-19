using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{

    private SceneKeyData sceneKeyData;
    public void RestartButton()
    {
		sceneKeyData = new SceneKeyData();

		sceneKeyData.sceneName = SceneManager.GetActiveScene().name;

		SceneChangeManager.Instance.SceneLoad(sceneKeyData);
    }
}
