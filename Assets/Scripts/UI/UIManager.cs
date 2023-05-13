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
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = instancePosition;
		return newUI;
	}
}
