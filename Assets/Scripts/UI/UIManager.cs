using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	[SerializeField]
	private GameObject modalBackgorund;

	//#����#	UI â�� �ν��Ͻ�ȭ�ϰ� �θ�� ��ġ�� �����ϴ� �Լ�
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

	//#����#	��� �ڽ� UIâ�� �ݴ� �Լ�
	public void UIWindowChildAllClose(Transform parentTransform)
	{
		foreach (Transform child in parentTransform)
		{
			if(child.CompareTag("UIWindow"))
			child.gameObject.GetComponent<UIWindowController>().UIWindowClose();
		}
	}
}
