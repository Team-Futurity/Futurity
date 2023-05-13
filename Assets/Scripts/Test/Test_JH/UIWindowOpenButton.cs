using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowOpenButton : MonoBehaviour
{
	[SerializeField]
	private GameObject OpenUiWindow;
	[SerializeField]
	private Vector2 instancePosition = Vector2.zero;

	
	public void UiOpenButtonClick()
	{
		if (OpenUiWindow)
		{
			UIManager.Instance.UIWindowOpen(OpenUiWindow, transform.parent, instancePosition);
		}
		else
		{
			FDebug.LogWarning($"{gameObject.name}의 UiOpenButtonClick에 OpenUiWindow가 존재하지 않습니다.");
		}
	}
}
