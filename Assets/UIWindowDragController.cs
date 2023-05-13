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


	void Start()
    {
		if (!parentUIWindow)
		{
			parentUIWindow = transform.parent.gameObject;
		}
		parentUIWindow.TryGetComponent<RectTransform>(out rectTransform);
		parentUIWindow.TryGetComponent<UIWindowController>(out uIWindowController);
	}

	//#설명#	UIWindow를 드래그 가능 하도록 드래그 시작 위치를 저장하고 UIWindow를 가장 앞으로 당겨오는 함수
	public void OnBeginDrag(PointerEventData eventData)
	{
		_pointerPosition = eventData.position;
		_windowPosition = rectTransform.localPosition;
		uIWindowController.BringToFront();
	}

	//#설명#	UIWindow를 드래그시 위치를 실시간으로 업데이트 시켜주는 함수
	public void OnDrag(PointerEventData eventData)
	{
		Vector2 pointerDelta = eventData.position - _pointerPosition;
		rectTransform.localPosition = _windowPosition + new Vector3(pointerDelta.x, pointerDelta.y, 0);
	}
}
