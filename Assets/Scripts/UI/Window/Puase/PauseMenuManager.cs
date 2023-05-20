using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PauseMenuManager : MonoBehaviour
{
	//#설명#	게임을 일시 정지시키고, PauseWindow를 출력합니다.


	[SerializeField]
	private GameObject pauseMenuWindow;
	private GameObject currentPauseMenuWindow;


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (Time.timeScale == 0)
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
		currentPauseMenuWindow = WindowManager.Instance.WindowTopOpen(pauseMenuWindow, Vector2.zero, Vector2.one);
	}

	//#설명#	게임 시간을 1로 설정하여 움직임을 원래대로 복구합니다.
	void Resume()
	{
		Time.timeScale = 1f; 
		WindowManager.Instance.WindowClose(pauseMenuWindow);
	}
}
