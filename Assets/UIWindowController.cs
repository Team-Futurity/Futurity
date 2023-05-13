using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//#����#	�ش� ��ũ��Ʈ�� UIWindow�� ������ �ִ� �ٽ� ��ũ��Ʈ��, UIWindow�� �������� �ൿ ����� ���ϰ� �ֽ��ϴ�.
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
