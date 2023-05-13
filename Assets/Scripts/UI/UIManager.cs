using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�
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

	//#����#	��� �ڽ� UIâ�� �ݴ� �Լ�
	public void UIWindowChildAllClose(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			if(child.CompareTag("UIWindow"))
			Destroy(child.gameObject);
		}
	}
}
