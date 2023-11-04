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
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		ChapterMoveController.Instance.EnableInteractionUI(EUIType.NEXTSTAGE);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		ChapterMoveController.Instance.DisableInteractionUI(EUIType.NEXTSTAGE);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck);
	}

	private void InputCheck(InputAction.CallbackContext context)
	{
		ChapterMoveController.Instance.DisableInteractionUI(EUIType.NEXTSTAGE);
		interactionEvent?.Invoke();
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck);
		
		gameObject.GetComponent<BoxCollider>().enabled = false;
	}
}
