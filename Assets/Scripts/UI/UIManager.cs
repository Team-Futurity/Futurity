using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수
	public GameObject UIWindowOpen(GameObject OpenUIWindowObject, Transform uiParent, Vector2 instancePosition)
	{
		GameObject newUI = Instantiate(OpenUIWindowObject, uiParent);
		if(!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = instancePosition;

		return newUI;
	}

	//#설명#	모든 자식 UI창을 닫는 함수
	public void UIWindowChildAllClose(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			if(child.CompareTag("UIWindow"))
			Destroy(child.gameObject);
		}
	}
}
