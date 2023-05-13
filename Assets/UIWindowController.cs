using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


//#����#	�ش� ��ũ��Ʈ�� UIWindow�� ������ �ִ� �ٽ� ��ũ��Ʈ��, UIWindow�� �������� �ൿ ����� ���ϰ� �ֽ��ϴ�.
public class UIWindowController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
{
	RectTransform rectTransform;
	//#����#
	private Vector2 _pointerPosition;
	private Vector3 _windowPosition;


	public void Start()
	{
		rectTransform = GetComponent<RectTransform>();
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

	public void OnBeginDrag(PointerEventData eventData)
	{
		_pointerPosition = eventData.position; 
		_windowPosition = rectTransform.localPosition;
		BringToFront();
	}
	public void OnDrag(PointerEventData eventData)
	{
		Vector2 pointerDelta = eventData.position - _pointerPosition;
		rectTransform.localPosition = _windowPosition + new Vector3(pointerDelta.x, pointerDelta.y, 0);
	}
}
