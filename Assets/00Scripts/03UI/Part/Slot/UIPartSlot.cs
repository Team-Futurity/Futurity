using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPartSlot : MonoBehaviour
{
	[SerializeField]
	private PartSystem partSystem;


	public UIPassiveSlot[] passiveSlots;
	public UIActiveSlot activeSlot;

	private void Awake()
	{
		// Part Slot과 Sync 맞추기
		SyncSlots();
	}

	public void SetPassiveSlot(int index, Sprite partIcon) => passiveSlots[index].SetSlot(partIcon);

	public void SetActiveSlot(Sprite partIcon) => activeSlot.SetSlot(partIcon);

	public void SyncSlots()
	{
	}

	public void ClearSlots()
	{
		foreach(var slot in passiveSlots)
		{
			slot.ClearSlot();
		}

		activeSlot.ClearSlot();
	}
	
}
