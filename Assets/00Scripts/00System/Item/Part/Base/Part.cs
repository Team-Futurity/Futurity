using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Part : MonoBehaviour
{
	[field: Header("UI 单捞磐")]
	[field: SerializeField] public ItemUIData PartUIData { get; private set; }
	[field: Header("何前 单捞磐")]
	[field: SerializeField] public PartData PartItemData { get; private set; }

	protected bool isActive = false;

	public void SetActive(bool isTrigger)
	{
		isActive = isTrigger;
	}

	public bool GetActive()
	{
		return isActive;
	}
}