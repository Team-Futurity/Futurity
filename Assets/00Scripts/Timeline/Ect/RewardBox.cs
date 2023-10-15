using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardBox : MonoBehaviour
{
	public bool isEnable = false;

	public UIPassivePartSelect passivePartSelect;
	private void OnTriggerStay(Collider other)
	{
		if (isEnable == false)
		{
			return;
		}
		
		if (other.CompareTag("Player"))
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				Debug.Log("??");

				passivePartSelect.SetPartData(2101, 2102, 2103);
				UIManager.Instance.OpenWindow(WindowList.PASSIVE_PART);

				InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
			}
		}
	}
}
