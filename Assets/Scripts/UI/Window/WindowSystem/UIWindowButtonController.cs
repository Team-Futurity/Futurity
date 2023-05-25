using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIWindowButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	[Header("윈도우의 버튼이 선텍되었을 경우 호출할 함수가 있을때 사용하는 스크립트")]
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
