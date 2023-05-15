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


	//#����#	���� �ð��� 0���� �����Ͽ� ��� �������� ����ϴ�.
	void Pause()
	{
		Time.timeScale = 0f;
		isPaused = true;
		currentPauseMenuWindow = UIWindowManager.Instance.UIWindowTopOpen(pauseMenuWindow, Vector2.zero, Vector2.one);
	}

	//#����#	���� �ð��� 1�� �����Ͽ� �������� ������� �����մϴ�.
	void Resume()
	{
		Time.timeScale = 1f; 
		isPaused = false;
		Destroy(currentPauseMenuWindow);
	}
}
