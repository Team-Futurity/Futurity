using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIWindowOpenController : MonoBehaviour
{
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



	//#����#	���ο� UI�� �ν��Ͻ�ȭ ��Ų��.
	public void UiOpen()
	{
		GameObject instanceUi;
		if (openUiWindow)
		{
			if (isWindowLock)
			{
				instanceUi = UIWindowManager.Instance.UIWindowTopOpen(openUiWindow, windowPosition, windowScale);
			}
			else
			{
				if (openUiWindowParent)
				{
					instanceUi = UIWindowManager.Instance.UIWindowOpen(openUiWindow, openUiWindowParent, windowPosition, windowScale);
				}
				else
				{
					instanceUi = UIWindowManager.Instance.UIWindowOpen(openUiWindow, transform.parent, windowPosition, windowScale);
				}
			}

			SetUiEvents(instanceUi);
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}�� UiOpenButtonClick�� OpenUiWindow�� �������� �ʽ��ϴ�.");
		}
	}

	//#����#	�̺�Ʈ ����
	private void SetUiEvents(GameObject instanceUi)
	{
		UIWindowController windowController = instanceUi.GetComponent<UIWindowController>();
		windowController.isLock = isWindowLock;

		for (int i = 0; i < windowEvents.Length; i++)
		{
			if (windowEvents[i].GetPersistentEventCount() > 0)
			{
				windowController.windowEvents[i] = windowEvents[i];
				FDebug.Log($"{i}��° �Ҵ� {windowEvents[i]}");
			}
		}
	}
}
