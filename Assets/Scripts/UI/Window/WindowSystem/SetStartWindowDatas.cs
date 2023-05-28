using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SetStartWindowDatas : MonoBehaviour
{
	//#����#	�ν����Ϳ��� �� InputActionReference�� �Ҵ����ּ���.
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
