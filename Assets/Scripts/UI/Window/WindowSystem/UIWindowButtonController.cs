using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIWindowButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	[Header("�������� ��ư Evnet�� Click �̿ܿ� �߰� ȿ���� �ʿ��� ��� ����ϴ� ��ũ��Ʈ")]
	[Space (15)]


	[SerializeField]
	private UnityEvent onButtonClickedEvents;
	[SerializeField]
	private UnityEvent offButtonClickedEvents;
	[SerializeField]
	private UnityEvent onHoldButtonEvents;
	[SerializeField]
	private UnityEvent offHoldButtonEvents;

	public void OnSelect(BaseEventData eventData)
	{
		onButtonClickedEvents?.Invoke();
	}
	public void OnDeselect(BaseEventData eventData)
	{
		offButtonClickedEvents?.Invoke();
	}

	public void OnHold()
	{
		onHoldButtonEvents?.Invoke();
	}
	public void OnDehold()
	{
		offHoldButtonEvents?.Invoke();
	}
}
