using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//#����#	�ش� ��ũ��Ʈ�� UIWindow�� ������ �ִ� �ٽ� ��ũ��Ʈ��, UIWindow�� �������� �ൿ ����� ���ϰ� �ֽ��ϴ�.
public class UIWindowDragController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
	[SerializeField]
	GameObject parentUIWindow;
	[SerializeField]
	RectTransform rectTransform;
	[SerializeField]
	UIWindowController uIWindowController;

	//#����#	���콺 �����Ϳ� Window�� Position��
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

	//#����#	UIWindow�� �巡�� ���� �ϵ��� �巡�� ���� ��ġ�� �����ϰ� UIWindow�� ���� ������ ��ܿ��� �Լ�
	public void OnBeginDrag(PointerEventData eventData)
	{
		_pointerPosition = eventData.position;
		_windowPosition = rectTransform.localPosition;
		uIWindowController.BringToFront();
	}

	//#����#	UIWindow�� �巡�׽� ��ġ�� �ǽð����� ������Ʈ �����ִ� �Լ�
	public void OnDrag(PointerEventData eventData)
	{
		Vector2 pointerDelta = eventData.position - _pointerPosition;
		rectTransform.localPosition = _windowPosition + new Vector3(pointerDelta.x, pointerDelta.y, 0);
	}
}
