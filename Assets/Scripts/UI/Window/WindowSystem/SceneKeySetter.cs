using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SceneKeySetter : MonoBehaviour
{
	//#설명#	인스팩터에서 각 InputActionReference를 할당해주세요.
	public InputActionReference leftAction;
	public InputActionReference rightAction;
	public InputActionReference selectAction;
	
	private void Start()
	{
		// WindowManager -> Input Key -> Setter
		WindowManager.Instance.SetInputActionReference(leftAction, rightAction, selectAction);
		
		// Current Window Clear
		WindowManager.Instance.ClearWindow();
		
		// Window Setter
		WindowManager.Instance.SetWindow(this.gameObject);
	}

	public void OnSetter()
	{
		this.enabled = true;
	}
}
