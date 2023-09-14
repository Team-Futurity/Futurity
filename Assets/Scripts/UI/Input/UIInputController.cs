using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputController : MonoBehaviour
{
	public void SetUp()
	{
		InputActionManager.Instance.DisableAllInputActionAsset();

		InputActionManager.Instance.EnableInputActionAsset(InputActionType.UI);

		SetKeyAction();
	}

	public void SetKeyAction()
	{
		
	}

	public void OnMoveToNextUI()
	{
		Debug.Log("TEST");
	}

}
