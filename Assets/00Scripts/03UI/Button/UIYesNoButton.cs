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

	public GameObject obj;

	protected override void ActiveFunc()
	{
		onSelected?.Invoke(isEquip);
	}

	public override void SelectActive(bool isOn)
	{
		obj.SetActive(isOn);
	}
}
