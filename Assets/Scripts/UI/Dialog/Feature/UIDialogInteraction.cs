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

		controller.OnStarted.AddListener(EnableKey);
		controller.OnEnded.AddListener(DisableKey);
	}

	private void PassDialog()
	{
		if (controller.currentState != DialogSystemState.NONE || controller.currentState != DialogSystemState.MAX)
		{
			Debug.Log("KEY ют╥б");
			controller.EnterNextInteraction();

			return;
		}
	}

	public void DisableKey()
	{
		passKey.Disable();
	}

	public void EnableKey()
	{
		passKey.Enable();
	}
}
