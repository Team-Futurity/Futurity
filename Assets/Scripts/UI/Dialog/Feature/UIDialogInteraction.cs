using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIDialogInteraction : UIDialogFeatureBase
{
	[field :SerializeField]
	public Button PassButton { get; private set; }

	[SerializeField]
	private InputAction passKey;

	protected override void Awake()
	{
		base.Awake();

		passKey.Enable();

		passKey.performed += (x) =>
		{
			PassDialog();
		};
	}

	private void PassDialog()
	{
		if (controller.currentState != DialogSystemState.NONE || controller.currentState != DialogSystemState.MAX)
		{
			controller.EnterNextInteraction();
		}
	}

	private void OnDisable()
	{
		passKey.Disable();
	}
}
