using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowController : MonoBehaviour
{

	public void UIWindowNewOpen(GameObject OpenUIWindowObject)
    {
		RectTransform rectTransform = GetComponent<RectTransform>();

		UIManager.Instance.UIWindowOpen(OpenUIWindowObject, transform.parent, rectTransform.localPosition + new Vector3(50, -50, 0));
    }

	public void UIWindowClose()
	{
		Destroy(this.gameObject);
	}
	public void UIWindowSiblingAllClose()
	{
		UIManager.Instance.UIWindowChildAllClose(transform.parent);
	}
}
