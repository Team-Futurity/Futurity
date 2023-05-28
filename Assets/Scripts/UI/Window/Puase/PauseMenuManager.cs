using UnityEngine.InputSystem;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
	[SerializeField]
	private InputActionReference pauseActionReference;

	[SerializeField]
	private GameObject pauseMenuWindow;
	private GameObject currentPauseMenuWindow;
	private int windowNum;


	private void OnEnable()
	{
		pauseActionReference.action.Enable();
	}

	private void OnDisable()
	{
		pauseActionReference.action.Disable();
	}

	void Awake()
	{
		pauseActionReference.action.performed += _ => TogglePause();
		windowNum = WindowManager.Instance.WindowPooling(pauseMenuWindow);
	}

	private void TogglePause()
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

	void Pause()
	{
		Time.timeScale = 0f;
		currentPauseMenuWindow = WindowManager.Instance.WindowOpen(windowNum, Vector2.zero, Vector2.one);
	}

	void Resume()
	{
		Time.timeScale = 1f;
		WindowManager.Instance.WindowClose(windowNum);
	}
}
