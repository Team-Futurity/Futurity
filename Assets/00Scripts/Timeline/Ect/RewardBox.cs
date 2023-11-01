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
	private bool isEnter;

	[Header("Open Animation")] 
	[SerializeField] private Animation boxAnimations;
	[SerializeField] private float waitTime = 0.6f;
	private IEnumerator startAnimation;

	private void OnDisable()
	{
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			StartAnimation();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		isEnter = true;
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") == false)
		{
			return;
		}
		
		isEnter = false;
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
	}

	private void OnInteractRewardBox(InputAction.CallbackContext context)
	{
		if (UIManager.Instance.IsOpenWindow(WindowList.PASSIVE_PART))
		{
			return;
		}
		
		passivePartSelect.SetPartData(partCodes);
		UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);

		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		gameObject.SetActive(false);
	}

	private void StartAnimation()
	{
		startAnimation = PlayAnimation();
		StartCoroutine(startAnimation);
	}

	private IEnumerator PlayAnimation()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		boxAnimations.Play();
		
		yield return new WaitForSeconds(waitTime);
		Debug.Log("Open Done");
	}
}
