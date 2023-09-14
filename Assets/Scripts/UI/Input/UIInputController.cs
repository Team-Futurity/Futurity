using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInputController : MonoBehaviour
{
	public void SetUp()
	{
		InputActionManager.Instance.DisableAllInputActionAsset();
		InputActionManager.Instance.EnableInputActionAsset(InputActionType.UI);
	}

	public void OnMoveToNextUI()
	{
		Debug.Log("Next UI");
	}

	public void OnMoveToPreviousUI()
	{
		Debug.Log("Previous UI");
	}

	public void OnSelectUI()
	{
		Debug.Log("Select UI");
	}
}
