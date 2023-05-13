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

	//#설명#	모든 자식 UI창을 닫는 함수
	public void CloseAllChildWindows(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			Destroy(child.gameObject);
		}
	}

	//#설명#	특정 태그를 가진 UI창을 닫는 함수
	public void CloseAllWindowsWithTag(string tag)
	{
		//#보완#	ObjectPull을 사용하도록 변경해볼것
		GameObject[] taggedUIWindows = GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject taggedUIWindow in taggedUIWindows)
		{
			Destroy(taggedUIWindow);
		}
	}
}
