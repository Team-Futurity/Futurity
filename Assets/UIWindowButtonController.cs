using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIWindowButtonController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
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
