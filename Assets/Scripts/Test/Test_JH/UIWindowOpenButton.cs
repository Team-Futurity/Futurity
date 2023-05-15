using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIWindowOpenButton : MonoBehaviour
{
	[SerializeField]
	private GameObject openUiWindow;
	[SerializeField]
	private Transform openUiWindowParent;
	[SerializeField]
	private Vector2 instancePosition = Vector2.zero;
	[SerializeField]
	private Vector2 windowScale = Vector2.one;
	[SerializeField]
	private bool isWindowLock = false;
	[SerializeField]
	private UnityEvent[] windowEvents = new UnityEvent[8];

	//#설명#	세로운 UI를 인스턴스화 시킨다.
	public void UiOpenButtonClick()
	{
		GameObject instanceUi;
		if (openUiWindow)
		{
			if (isWindowLock)
			{
				instanceUi = UIWindowManager.Instance.UIWindowTopOpen(openUiWindow, instancePosition, windowScale);
			}
			else
			{
				if (openUiWindowParent)
				{
					instanceUi = UIWindowManager.Instance.UIWindowOpen(openUiWindow, openUiWindowParent, instancePosition, windowScale);
				}
				else
				{
					instanceUi = UIWindowManager.Instance.UIWindowOpen(openUiWindow, transform.parent, instancePosition, windowScale);
				}
			}

			SetUiEvent(instanceUi);
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}
	}

	//#설명#	이벤트 전달
	private void SetUiEvent(GameObject instanceUi)
	{
		UIWindowController windowController = instanceUi.GetComponent<UIWindowController>();
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
