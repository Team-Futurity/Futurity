using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    public string _sceneName;
    public void RestartButton()
    {
		SceneChanger.Instance.SceneLoader(_sceneName);
    }
}
