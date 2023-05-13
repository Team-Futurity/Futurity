using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowOpenButton : MonoBehaviour
{
	[SerializeField]
	private GameObject OpenUiWindow;
	[SerializeField]
	private Vector2 instancePosition = Vector2.zero;
	[SerializeField]
	private bool isUiLock = false;


	public void UiOpenButtonClick()
	{
		if (OpenUiWindow)
		{
			if(isUiLock)
			{
				UIManager.Instance.UIWindowOpen(OpenUiWindow, transform.parent, instancePosition).GetComponent<UIWindowController>().isLock = true;
			}
			else
			{
				UIManager.Instance.UIWindowOpen(OpenUiWindow, transform, instancePosition).GetComponent<UIWindowController>().isLock = false;
			}
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}
	}
}
