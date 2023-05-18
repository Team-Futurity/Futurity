using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindowOpenController : MonoBehaviour
{
	//#설명#	해당 스크립트는 특정 Window를 열때 사용하는 스크립트입니다.
	[Header ("해당 스크립트는 특정 Window를 열때 사용하는 스크립트입니다.")]
	[Space(15)]


	[SerializeField]
	private GameObject openUiWindow;
	[SerializeField]
	private Transform openUiWindowParent;
	[SerializeField]
	private Vector2 windowPosition = Vector2.zero;
	[SerializeField]
	private Vector2 windowScale = Vector2.one;
	[SerializeField]
	private bool isWindowLock = false;
	[SerializeField]
	private UnityEvent[] windowEvents = new UnityEvent[8];



	public void WindowOpen()
	{
		//#설명#	세로운 UI를 인스턴스화.
		GameObject instanceUi;
		if (openUiWindow)
		{
			if (isWindowLock)
			{
				instanceUi = WindowManager.Instance.WindowTopOpen(openUiWindow, windowPosition, windowScale);
			}
			else
			{
				if (openUiWindowParent)
				{
					instanceUi = WindowManager.Instance.WindowOpen(openUiWindow, openUiWindowParent, windowPosition, windowScale);
				}
				else
				{
					instanceUi = WindowManager.Instance.WindowOpen(openUiWindow, transform.parent, windowPosition, windowScale);
				}
			}

			SetWindowEvents(instanceUi);
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}
	}


	private void SetWindowEvents(GameObject instanceUi)
	{
		//#설명#	생성한 Window에 이벤트 전달.
		WindowController windowController = instanceUi.GetComponent<WindowController>();
		windowController.isLock = isWindowLock;

		for (int i = 0; i < windowEvents.Length; i++)
		{
			if (windowEvents[i].GetPersistentEventCount() > 0)
			{
				windowController.windowEvents[i] = windowEvents[i];
				FDebug.Log($"{i}번째 할당 {windowEvents[i]}");
			}
		}
	}
}
