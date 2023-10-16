using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewardBox : MonoBehaviour
{
	public UIPassivePartSelect passivePartSelect;

	public int[] partCodes;

	private bool isEnter;

	private void Start()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isEnter = true;
			InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isEnter = false;
			InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.Player.Interaction, OnInteractRewardBox, true);
		}
	}

	public void OnInteractRewardBox(InputAction.CallbackContext context)
	{
		if (UIManager.Instance.IsOpenWindow(WindowList.PASSIVE_PART)) { return; }

		passivePartSelect.SetPartData(partCodes);
		UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);

		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
	}
}
