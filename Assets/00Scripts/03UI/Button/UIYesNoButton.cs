using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIYesNoButton : UIButton
{
	[SerializeField]
	private bool isNo;

	[HideInInspector]
	public UnityEvent<bool> onActive;

	protected override void ActiveFunc()
	{
		onActive?.Invoke(!isNo);
	}
}
