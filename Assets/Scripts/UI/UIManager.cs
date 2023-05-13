using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject uiObject;

	public void CreateUIElementWithWidth(Vector2 uiSize, Transform uiParent, Vector2 instancePosition)
	{
		GameObject newUI = Instantiate(uiObject, uiParent);
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(uiSize.x, uiSize.y);
		rectTransform.localPosition = instancePosition;
	}
}
