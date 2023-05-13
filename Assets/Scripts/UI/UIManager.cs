using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	[SerializeField]
	private GameObject modalBackgorund;

	//#설명#	UI 창을 인스턴스화하고 부모와 위치를 설정하는 함수
	public GameObject UIWindowOpen(GameObject OpenUiWindowObject, Transform uiParent, Vector2 instancePosition)
	{
		GameObject newUI = Instantiate(OpenUiWindowObject, uiParent);
		if(!newUI.CompareTag("UIWindow"))
		{
			newUI.tag = "UIWindow";
		}
		RectTransform rectTransform = newUI.GetComponent<RectTransform>();
		rectTransform.localPosition = instancePosition;
		newUI.GetComponent<UIWindowController>().parentObject = uiParent.gameObject;

		return newUI;
	}

	//#설명#	모든 자식 UI창을 닫는 함수
	public void UIWindowChildAllClose(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			if(child.CompareTag("UIWindow"))
			child.gameObject.GetComponent<UIWindowController>().UIWindowClose();
		}
	}
}
