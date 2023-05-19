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
	private GameObject openWindow;
	[SerializeField]
	private Transform openWindowParent;
	[SerializeField]
	private Vector2 windowPosition = Vector2.zero;
	[SerializeField]
	private Vector2 windowScale = Vector2.one;
	[SerializeField]
	private UnityEvent[] windowEvents = new UnityEvent[8];



	public void WindowOpen()
	{
		//#����#	���ο� UI�� �ν��Ͻ�ȭ.
		GameObject instanceUi;
		if (openWindow)
		{
				if (openWindowParent)
				{
					instanceUi = WindowManager.Instance.WindowOpen(openWindow, openWindowParent, windowPosition, windowScale);
				}
				else
				{
					instanceUi = WindowManager.Instance.WindowTopOpen(openWindow, windowPosition, windowScale);
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
