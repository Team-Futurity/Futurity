using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowOpenButton : MonoBehaviour
{
	[SerializeField]
	private GameObject OpenUIWindow;
	[SerializeField]
	private Vector2 instancePosition = Vector2.zero;

	public void UiOpenButtonClick()
	{
		UIManager.Instance.UIWindowOpen(OpenUIWindow, transform.parent, instancePosition);
	}
}
