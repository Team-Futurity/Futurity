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
			FDebug.Log("�߸��� �����Դϴ�.");
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
		DisableSelectEvent();

		Equip.SetSelectPart(code);
		Equip.SyncPartDataToPartSystem();

		// �ݱ� -> Passive Part Window
		UIManager.Instance.CloseWindow(WindowList.PASSIVE_PART);
		
		// ���� -> Part Equip Window
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
