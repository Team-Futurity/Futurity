using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
	public UIPassivePartSelect passivePartSelect;

	public int[] partCodes;

	private void Start()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.T) && !UIManager.Instance.IsOpenWindow(WindowList.PASSIVE_PART))
			{
				passivePartSelect.SetPartData(partCodes);
				UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);

				InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
			}
		}
	}
}
