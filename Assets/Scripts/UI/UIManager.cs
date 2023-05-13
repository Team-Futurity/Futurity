using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	//#����#	UI�����
	private Dictionary<string, List<GameObject>> uiWindows;


	//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�
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

	//#����#	��� �ڽ� UIâ�� �ݴ� �Լ�
	public void CloseAllChildWindows(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			Destroy(child.gameObject);
		}
	}

	//#����#	Ư�� �±׸� ���� UIâ�� �ݴ� �Լ�
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
