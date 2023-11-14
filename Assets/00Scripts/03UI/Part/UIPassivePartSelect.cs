using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPassivePartSelect : MonoBehaviour
{
	public UIPartSelectButton[] passiveSelectButtons;

	[field: SerializeField, Space(15)]
	public UIPartEquip Equip { get; private set; }

	public List<GameObject> canvasList;

	private void OnEnable()
	{
		if (canvasList == null && !UIManager.Instance.IsOpenWindow(WindowList.PASSIVE_PART))
			return;
		
		foreach (var canvas in canvasList)
		{
			canvas.SetActive(false);
		}
	}

	public void SetPartData(params int[] partCodes)
	{
		UIManager.Instance.CloseDefaultWindow();

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
		DisableSelectEvent();

		Equip.SetSelectPart(code);
		Equip.SyncPartDataToPartSystem();

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
