using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Part : MonoBehaviour
{
	[field: SerializeField] public ItemUIData PartUIData { get; private set; }
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