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
	private GameObject openWindow;
	[SerializeField]
	private Transform openWindowParent;
	[SerializeField]
	private Vector2 windowPosition = Vector2.zero;
	[SerializeField]
	private Vector2 windowScale = Vector2.one;
	[SerializeField]
	public UnityEvent[] windowEvents = new UnityEvent[8];
	[SerializeField]
	private Dictionary<string, object> variables = new Dictionary<string, object>();


	/// <summary>
	/// 타 Window를 닫는 Window를 생성
	/// </summary>
	public void WindowDeactiveOpen()
	{
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
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}

		SetWindowEvents(instanceUi);
		SetVariablesToWindow(instanceUi);
	}
	public void WindowDeactiveOpen(GameObject openWindowPrefab)
	{
		openWindow = openWindowPrefab;

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
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}

		SetWindowEvents(instanceUi);
		SetVariablesToWindow(instanceUi);
	}

	/// <summary>
	/// 타 Window를 닫지 않는 Window를 생성
	/// </summary>
	public void WindowActiveOpen()
	{
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
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}

		SetWindowEvents(instanceUi);
		SetVariablesToWindow(instanceUi);
	}
	public void WindowActiveOpen(GameObject openWindowPrefab)
	{
		openWindow = openWindowPrefab;

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
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}

		SetWindowEvents(instanceUi);
		SetVariablesToWindow(instanceUi);
	}

	/// <summary>
	/// 타 Window를 닫지 않는 Window를 생성
	/// </summary>
	public GameObject WindowActiveReturnOpen()
	{
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
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}

		SetWindowEvents(instanceUi);
		SetVariablesToWindow(instanceUi);

		return instanceUi;
	}

	/// <summary>
	/// 생성한 Window에 이벤트 전달.
	/// </summary>
	/// <param name="instanceUi">Window에 필요한 함수를 호출</param>
	private void SetWindowEvents(GameObject instanceUi)
	{
		WindowController windowController = instanceUi.GetComponent<WindowController>();

		for (int i = 0; i < windowEvents.Length; i++)
		{
			if (windowEvents[i].GetPersistentEventCount() > 0)
			{
				windowController.windowEvents[i] = windowEvents[i];
				FDebug.Log($"{i}번째 할당 {windowEvents[i]}");
			}
		}
	}

	#region Variable 관련
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
	#endregion
}
