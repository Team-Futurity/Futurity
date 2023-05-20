using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PauseMenuManager : MonoBehaviour
{
	//#����#	������ �Ͻ� ������Ű��, PauseWindow�� ����մϴ�.


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


	//#����#	���� �ð��� 0���� �����Ͽ� ��� �������� ����ϴ�.
	void Pause()
	{
		Time.timeScale = 0f;
		currentPauseMenuWindow = WindowManager.Instance.WindowTopOpen(pauseMenuWindow, Vector2.zero, Vector2.one);
	}

	//#����#	���� �ð��� 1�� �����Ͽ� �������� ������� �����մϴ�.
	void Resume()
	{
		Time.timeScale = 1f; 
		WindowManager.Instance.WindowClose(pauseMenuWindow);
	}
}
