using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIDialogPass : UIDialogFeatureBase
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
			OnPass();
		};
	}

	public void SetKey(InputAction input)
	{
		//// 추후, KeyChanger에서 가져올 수 있도록 한다.
		//passKey = input;
	}

	private void OnPass()
	{
		if (controller.GetActive())
		{
			controller.OnPass();
		}
	}

	private void OnDisable()
	{
		passKey.Disable();
	}
}
