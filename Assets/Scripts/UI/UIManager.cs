using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	//#설명#	UI저장소
	private Dictionary<string, List<GameObject>> uiWindows;


	//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수
	public GameObject UIWindowOpen(GameObject OpenUIWindowObject, Transform uiParent, Vector2 instancePosition)
	{
		GameObject newUI = Instantiate(OpenUIWindowObject, uiParent);
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = instancePosition;

		if (!string.IsNullOrEmpty(tag))
		{
			newUI.tag = tag;
			if (!uiWindows.ContainsKey(tag))
			{
				uiWindows[tag] = new List<GameObject>();
			}
			uiWindows[tag].Add(newUI);
		}

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
		if (uiWindows.ContainsKey(tag))
		{
			foreach (GameObject taggedUIWindow in uiWindows[tag])
			{
				Destroy(taggedUIWindow);
			}
			uiWindows[tag].Clear();
		}
	}
}
