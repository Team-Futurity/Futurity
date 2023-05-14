using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public string _sceneName;
    public void RestartButton()
    {
		SceneChangeManager.Instance.SceneLoader(_sceneName);
    }
}
