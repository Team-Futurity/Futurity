using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseMenuWindowManager : MonoBehaviour
{
	[SerializeField]
	float upWidthValue;

	public void PauseRelease()
	{
		Time.timeScale = 1f;
		UIWindowManager.Instance.UIWindowClose(gameObject);
	}

	public void PauseUIButtonUpWidth(GameObject upWidthUI)
	{
		RectTransform uiRectTransform = upWidthUI.GetComponent<RectTransform>();

		uiRectTransform.sizeDelta = new Vector2(uiRectTransform.sizeDelta.x + upWidthValue, uiRectTransform.sizeDelta.y);
	}
	public void PauseUIButtonDownWidth(GameObject upWidthUI)
	{
		RectTransform uiRectTransform = upWidthUI.GetComponent<RectTransform>();

		uiRectTransform.sizeDelta = new Vector2(uiRectTransform.sizeDelta.x - upWidthValue, uiRectTransform.sizeDelta.y);
	}
}
