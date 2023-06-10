using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SetStartWindowDatas : MonoBehaviour
{
	//#설명#	인스팩터에서 각 InputActionReference를 할당해주세요.
	public InputActionReference leftAction;
	public InputActionReference rightAction;
	public InputActionReference selectAction;

	void Start()
	{
		WindowManager.Instance.SetInputActionReference(leftAction, rightAction, selectAction);
		WindowManager.Instance.ClearWindow();
		WindowManager.Instance.SetWindow(this.gameObject);
	}
}
