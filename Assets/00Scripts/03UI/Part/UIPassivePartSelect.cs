using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPassivePartSelect : MonoBehaviour
{
	public UIPartSelectButton[] passiveSelectButtons;
	
	[field: SerializeField, Space(15)]
	public UIPartEquip Equip { get; private set; }

	public void SetPartData(params int[] partCodes)
	{
		if (partCodes.Length > 3)
		{
			FDebug.Log("잘못된 인자입니다.");
			FDebug.Break();
		}

		for (int i = 0; i < passiveSelectButtons.Length; ++i)
		{
			passiveSelectButtons[i].SetButtonData(partCodes[i]);
		}

		EnableSelectEvent();
	}

	private void OpenEquipWindow(int code, int index)
	{
		Debug.Log("Equip Window Open " + code + " : " + index);

		Equip.SetSelectPart(code);

		DisableSelectEvent();

		// 닫기 -> Passive Part Window
		UIManager.Instance.CloseWindow(WindowList.PASSIVE_PART);
		
		// 열기 -> Part Equip Window
		UIManager.Instance.OpenWindow(WindowList.PART_EQUIP);
	}

	private void EnableSelectEvent()
	{
		for (int i = 0; i < passiveSelectButtons.Length; ++i)
		{
			passiveSelectButtons[i].onSelected?.AddListener(OpenEquipWindow);
		}
	}

	private void DisableSelectEvent()
	{
		for(int i = 0;i<passiveSelectButtons.Length;++i)
		{
			passiveSelectButtons[i].onSelected.RemoveAllListeners();
		}
	}
}
