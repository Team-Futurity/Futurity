using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UIPartSlot : MonoBehaviour
{
	[SerializeField]
	private PartSystem partSystem;

	public UIPassiveSlot[] passiveSlots;
	public UIActiveSlot activeSlot;

	// ���� String -> ID�� �������� Addressable�� Item Data�� �����´�.

	private void Awake()
	{
		// Part Equip
		partSystem.onPartEquip?.AddListener((x, y) =>
	   {
		   var image = LoadPartIconImage(x);
		   SetPassiveSlot(y, image);
	   });
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

	private Sprite LoadPartIconImage(int code)
	{
		var spriteImage = Addressables.LoadAssetAsync<Sprite>(code).WaitForCompletion();
		return spriteImage;
	}
	
}
