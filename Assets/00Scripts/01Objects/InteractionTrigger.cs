using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractionTrigger : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private GameObject interactionUI;
	[SerializeField] private UnityEvent interactionEvent;
	
	private void Start()
	{
		interactionUI = ChapterMoveController.Instance.transform.GetChild(0).gameObject;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		interactionUI.SetActive(true);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		interactionUI.SetActive(false);
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck);
	}

	private void InputCheck(InputAction.CallbackContext context)
	{
		interactionUI.SetActive(false);
		interactionEvent?.Invoke();
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, InputCheck);
	}
}
