using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionTrigger : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private UnityEvent interactionEvent;
	[SerializeField] private GameObject interactionEffect;
	[SerializeField, ReadOnly(false)] private bool isInteraction = false;
	
	public void CheckInteraction()
	{
		if (isInteraction == true)
		{
			return;
		}

		gameObject.GetComponent<BoxCollider>().enabled = true;
		interactionEffect.SetActive(true);
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		ChapterMoveController.Instance.EnableInteractionUI(EUIType.INTERACION);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck, true);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		ChapterMoveController.Instance.DisableInteractionUI(EUIType.INTERACION);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck,true);
	}

	private void InputCheck(InputAction.CallbackContext context)
	{
		ChapterMoveController.Instance.DisableInteractionUI(EUIType.INTERACION);
		interactionEvent?.Invoke();
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck,true);
		
		gameObject.GetComponent<BoxCollider>().enabled = false;
		isInteraction = true;
	}
	
	private void OnEnable() => interactionEffect.SetActive(true);
}
