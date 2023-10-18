using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisableEvent : MonoBehaviour
{
	private void OnDisable()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
	}
}
