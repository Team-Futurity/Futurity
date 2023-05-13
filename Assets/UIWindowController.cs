using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//#설명#	해당 스크립트는 UIWindow가 가지고 있는 핵심 스크립트로, UIWindow의 전반적인 행동 양식을 지니고 있습니다.
public class UIWindowController : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	RectTransform rectTransform;


	public void Start()
	{
		TryGetComponent<RectTransform>(out rectTransform);
	}

	public void UIWindowNewOpen(GameObject OpenUIWindowObject)
    {
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
	public void OnPointerClick(PointerEventData eventData)
	{
		BringToFront();
	}

	public void BringToFront()
	{
		rectTransform.SetAsLastSibling();
	}
}
