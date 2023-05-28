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
	[SerializeField]
	private Dictionary<string, object> variables = new Dictionary<string, object>();




	public void WindowOpen()
	{
		//#����#	���ο� UI�� �ν��Ͻ�ȭ.
		GameObject instanceUi = null;
		if (openWindow)
		{
				if (openWindowParent)
				{
					instanceUi = WindowManager.Instance.WindowOpen(openWindow, openWindowParent, true, windowPosition, windowScale);
				}
				else
				{
					instanceUi = WindowManager.Instance.WindowTopOpen(openWindow, true, windowPosition, windowScale);
				}
			

			SetWindowEvents(instanceUi);
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}�� UiOpenButtonClick�� OpenUiWindow�� �������� �ʽ��ϴ�.");
		}

		SetWindowEvents(instanceUi);
		SetVariablesToWindow(instanceUi);
	}
	public GameObject WindowActiveOpen()
	{
		//#����#	���ο� UI�� �ν��Ͻ�ȭ.
		GameObject instanceUi = null;
		if (openWindow)
		{
				if (openWindowParent)
				{
					instanceUi = WindowManager.Instance.WindowOpen(openWindow, openWindowParent, false, windowPosition, windowScale);
				}
				else
				{
					instanceUi = WindowManager.Instance.WindowTopOpen(openWindow, false, windowPosition, windowScale);
				}
			

			SetWindowEvents(instanceUi);
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}�� UiOpenButtonClick�� OpenUiWindow�� �������� �ʽ��ϴ�.");
		}

		SetWindowEvents(instanceUi);
		SetVariablesToWindow(instanceUi);

		return instanceUi;
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


	public void AddVariable(string name, object value)
	{
		variables[name] = value;
	}
	private void SetVariablesToWindow(GameObject instanceUi)
	{
		WindowController windowController = instanceUi.GetComponent<WindowController>();

		foreach (var pair in variables)
		{
			windowController.SetVariable(pair.Key, pair.Value);
		}
	}
}
