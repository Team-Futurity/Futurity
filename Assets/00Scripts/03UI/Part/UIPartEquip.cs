using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPartEquip : MonoBehaviour
{
	// 0, 1, 2 -> Top, Middle, Bottom
	public List<UIPartSelectButton> passiveButton;

	[field: SerializeField]
	public PartSystem PartEquipSystem { get; private set; }
	
	[field: SerializeField]
	public UIPartEquipSelect SelectModal { get; private set; }
	
	private int selectPartCode = 0;
	private int selectButtonIndex = 0;

	private bool isSelect = false;

	private void Start()
	{
		UpdatePartData();
		AddButtonEvent();
	}

	public void SetSelectPart(int code)
	{
		selectPartCode = code;
		
		isSelect = true;
	}
	
	private void UpdatePartData()
	{
		var partDatas = PartEquipSystem.equipPartList;
		
		for (int i = 0; i < partDatas.Length; ++i)
		{
			if (partDatas[i] != null)
			{
				SetPartData(i, partDatas[i].partCode);
			}
		}
	}

	private void SetPartData(int index, int partCodes)
	{
		passiveButton[index].SetButtonData(partCodes);
	}

	private void AddButtonEvent()
	{
		Debug.Log("Enable");

		for (int i = 0; i < passiveButton.Count; ++i)
		{
			passiveButton[i].onActive?.AddListener(SelectButton);
		}
	}

	private void RemoveButtonEvent()
	{
		Debug.Log("Disable");
		
		for (int i = 0; i < passiveButton.Count; ++i)
		{
			passiveButton[i].onActive?.RemoveAllListeners();
		}
	}
	
	private void SelectButton(int partCode, int selectIndex)
	{
		// PartCode : 선택한 Part Code
		// SelectIndex : 선택한 Button의 Index
		var emptyPart = PartEquipSystem.IsPartEmpty(selectIndex);

		selectPartCode = partCode;
		selectButtonIndex = selectIndex;

		UIInputManager.Instance.SaveIndex();

		if (!isSelect)
		{
			return;
		}

		if (emptyPart)
		{
			ChangePart(false);
		}
		else
		{
			UIManager.Instance.OpenWindow(WindowList.PART_EQUIP_SELECT);
			SelectModal.onClose?.AddListener(ChangePart);
		}
		
	}
	
	// 부품을 변경하는 메서드
	private void ChangePart(bool isNo)
	{
		SelectModal.onClose?.RemoveAllListeners();
		
		if (isNo == false)
		{
			PartEquipSystem.EquipPart(selectButtonIndex, selectPartCode, true);
			UpdatePartData();
			
			isSelect = false;
		}
		
		UIManager.Instance.RefreshWindow(WindowList.PART_EQUIP);
		UIInputManager.Instance.SetSaveIndexToCurrentIndex();
	}
}