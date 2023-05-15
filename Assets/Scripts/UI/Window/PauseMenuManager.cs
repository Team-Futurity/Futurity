using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
	[SerializeField]
	private GameObject pauseMenuWindow;
	private GameObject currentPauseMenuWindow;
	private bool isPaused = false;


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (isPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}


	//#설명#	게임 시간을 0으로 설정하여 모든 움직임을 멈춤니다.
	void Pause()
	{
		Time.timeScale = 0f;
		isPaused = true;
		currentPauseMenuWindow = UIWindowManager.Instance.UIWindowTopOpen(pauseMenuWindow, Vector2.zero, Vector2.one);
	}

	//#설명#	게임 시간을 1로 설정하여 움직임을 원래대로 복구합니다.
	void Resume()
	{
		Time.timeScale = 1f; 
		isPaused = false;
		Destroy(currentPauseMenuWindow);
	}
}
