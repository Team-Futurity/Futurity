using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindowOpenController : MonoBehaviour
{
	//#����#	�ش� ��ũ��Ʈ�� Ư�� Window�� ���� ����ϴ� ��ũ��Ʈ�Դϴ�.
	[Header ("�ش� ��ũ��Ʈ�� Ư�� Window�� ���� ����ϴ� ��ũ��Ʈ�Դϴ�.")]
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
		//#����#	���ο� UI�� �ν��Ͻ�ȭ.
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
			FDebug.LogWarning($"{gameObject.name}�� UiOpenButtonClick�� OpenUiWindow�� �������� �ʽ��ϴ�.");
		}
	}


	private void SetWindowEvents(GameObject instanceUi)
	{
		//#����#	������ Window�� �̺�Ʈ ����.
		WindowController windowController = instanceUi.GetComponent<WindowController>();
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
