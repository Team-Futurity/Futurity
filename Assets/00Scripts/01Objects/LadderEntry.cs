using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderEntry : MonoBehaviour
{
	[SerializeField] private float fadeTime = 0.8f;
	[SerializeField] private GameObject interactionUI;
	[SerializeField] private Transform movePos;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckInput);
		interactionUI.SetActive(true);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckInput);
		interactionUI.SetActive(false);
	}

	private void CheckInput(InputAction.CallbackContext context)
	{
		DownLadder();
		interactionUI.SetActive(false);
		gameObject.SetActive(false);
	}

	private void DownLadder()
	{
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, CheckInput);
		Transform player = GameObject.FindWithTag("Player").transform;
		
		FadeManager.Instance.FadeIn(fadeTime, () =>
		{
			player.SetPositionAndRotation(movePos.position, movePos.rotation);
			Invoke(nameof(DelayFadeOut), fadeTime);
		});
	}

	private void DelayFadeOut()
	{
		FadeManager.Instance.FadeOut(fadeTime);
	}
}
