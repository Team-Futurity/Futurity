using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIWindowOpenButton : MonoBehaviour
{
	[SerializeField]
	private GameObject openUiWindow;
	[SerializeField]
	private Vector2 instancePosition = Vector2.zero;
	[SerializeField]
	private Vector2 windowScale = Vector2.zero;
	[SerializeField]
	private bool isWindowLock = false;
	[SerializeField]
	private UnityEvent[] windowEvents = new UnityEvent[8];


	public void UiOpenButtonClick()
	{
		GameObject instanceUi;
		if (openUiWindow)
		{
			if (isWindowLock)
			{
				instanceUi = UIWindowManager.Instance.UIWindowOpen(openUiWindow, transform.parent, instancePosition, windowScale);
			}
			else
			{
				instanceUi = UIWindowManager.Instance.UIWindowOpen(openUiWindow, transform.parent, instancePosition, windowScale);
			}

			UIWindowController windowController = instanceUi.GetComponent<UIWindowController>();
			windowController.isLock = isWindowLock;
			windowController.windowEvents = windowEvents;
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}�� UiOpenButtonClick�� OpenUiWindow�� �������� �ʽ��ϴ�.");
		}
	}
}
