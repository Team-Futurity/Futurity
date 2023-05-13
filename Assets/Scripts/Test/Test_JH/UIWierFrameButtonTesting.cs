using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWierFrameButtonTesting : MonoBehaviour
{
	[SerializeField]
	private UIManager uiManager;
	[SerializeField]
	private Vector2 uiSize = new Vector2 (200, 100);
	[SerializeField]
	private Vector2 instancePosition = Vector2.zero;

	public void UiOpenButtonClick()
	{
		uiManager.CreateUIElementWithWidth(uiSize, transform.parent, instancePosition);
	}
}
