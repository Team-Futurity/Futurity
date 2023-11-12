using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LadderEntry : MonoBehaviour
{
	[Header("Component")]
	[SerializeField] private GameObject interactionUI;
	[SerializeField] private SkeletonAnimation fenceSkeleton;
	[SerializeField] private RewardBox rewardBox;
	[SerializeField] private DialogPlayer dialogPlayer;
	
	[Header("설정값")]
	[SerializeField] private float fadeTime = 0.8f;
	[SerializeField] private float delayTime = 1.2f;
	[SerializeField] private Transform movePos;
	
	[Header("Event")]
	[SerializeField] private UnityEvent disableEvent;

	private IEnumerator downLadder;
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, StartDownLadder, true);
		interactionUI.SetActive(true);
	}
	
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, StartDownLadder, true);
		interactionUI.SetActive(false);
	}

	private void StartDownLadder(InputAction.CallbackContext context)
	{
		if (rewardBox.isInteraction == false)
		{
			dialogPlayer.StartPlayDialog(1);
			rewardBox.isInteraction = true;
			
			return;
		}
		
		disableEvent?.Invoke();
		interactionUI.SetActive(false);
		
		InputActionManager.Instance.DisableActionMap();
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, StartDownLadder);

		downLadder = DownLadder();
		StartCoroutine(downLadder);
	}

	private IEnumerator DownLadder()
	{
		Transform player = GameObject.FindWithTag("Player").transform;
		player.GetComponent<Rigidbody>().velocity = Vector3.zero;

		fenceSkeleton.AnimationState.SetAnimation(0, "bystreet_netfence_3", false);

		yield return new WaitForSeconds(delayTime);
		
		FadeManager.Instance.FadeIn(fadeTime, () =>
		{
			player.SetPositionAndRotation(movePos.position, movePos.rotation);
			Invoke(nameof(DelayFadeOut), fadeTime);
		});
	}

	private void DelayFadeOut()
	{
		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		});
	}
}
