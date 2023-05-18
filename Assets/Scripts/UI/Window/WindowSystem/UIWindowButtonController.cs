using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIWindowButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	[Header("�������� ��ư�� ���صǾ��� ��� ȣ���� �Լ��� ������ ����ϴ� ��ũ��Ʈ")]
	[Space (15)]


	[SerializeField]
	private UnityEvent onButtonClickedEvents;
	[SerializeField]
	private UnityEvent offButtonClickedEvents;

	public void OnSelect(BaseEventData eventData)
	{
		onButtonClickedEvents?.Invoke();
	}
	public void OnDeselect(BaseEventData eventData)
	{
		offButtonClickedEvents?.Invoke();
	}

}
