using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//#설명#	해당 스크립트는 UIWindow가 가지고 있는 핵심 스크립트로, UIWindow의 전반적인 행동 양식을 지니고 있습니다.
public class UIWindowDragController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	[SerializeField]
	GameObject parentUIWindow;
	[SerializeField]
	RectTransform rectTransform;
	[SerializeField]
	UIWindowController uIWindowController;

	//#설명#	마우스 포인터와 Window의 Position값
	private Vector2 _pointerPosition;
	private Vector3 _windowPosition;

	// Start is called before the first frame update
	void Start()
    {
		if (!parentUIWindow)
		{
			parentUIWindow = transform.parent.gameObject;
		}
		parentUIWindow.TryGetComponent<RectTransform>(out rectTransform);
		parentUIWindow.TryGetComponent<UIWindowController>(out uIWindowController);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		_pointerPosition = eventData.position;
		_windowPosition = rectTransform.localPosition;
		uIWindowController.BringToFront();
	}
	public void OnDrag(PointerEventData eventData)
	{
		Vector2 pointerDelta = eventData.position - _pointerPosition;
		rectTransform.localPosition = _windowPosition + new Vector3(pointerDelta.x, pointerDelta.y, 0);
	}
}
