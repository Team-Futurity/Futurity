using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIYesNoButton : UIButton
{
	[SerializeField]
	private bool isEquip;

	[HideInInspector]
	public UnityEvent<bool> onSelected;

	protected override void ActiveFunc()
	{
		onSelected?.Invoke(isEquip);
	}
}
