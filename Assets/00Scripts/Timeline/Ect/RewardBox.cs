using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
	public UIPassivePartSelect passivePartSelect;

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
				passivePartSelect.SetPartData(2101, 2102, 2103);
				UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);

				InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
			}
		}
	}
}
