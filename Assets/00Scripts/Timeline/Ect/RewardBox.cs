using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewardBox : MonoBehaviour
{
	[Header("부품 컴포넌트")]
	public UIPassivePartSelect passivePartSelect;
	public int[] partCodes;

	[Header("Open Animation")] 
	[SerializeField] private Animation boxAnimations;
	[SerializeField] private float waitTime = 0.6f;
	private IEnumerator startAnimation;
	
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnInteractRewardBox(InputAction.CallbackContext context)
	{
		startAnimation = PlayAnimation();
		StartCoroutine(startAnimation);
	}
	
	private IEnumerator PlayAnimation()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		boxAnimations.Play();
		
		yield return new WaitForSeconds(waitTime);
		
		if (UIManager.Instance.IsOpenWindow(WindowList.PASSIVE_PART))
		{ 
			yield return null;
		}
		
		passivePartSelect.SetPartData(partCodes);
		UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);
		gameObject.GetComponent<BoxCollider>().enabled = false;
	}
}
