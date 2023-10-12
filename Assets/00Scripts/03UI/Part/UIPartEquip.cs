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
	}

	private void OnEnable()
	{
		AddButtonEvent();
	}

	private void OnDisable()
	{
		RemoveButtonEvent();
	}
	
	public void SetSelectPart(int code)
	{
		selectPartCode = code;
		isSelect = true;
	}

	private void UpdatePartData()
	{
		var partDatas = PartEquipSystem.equipPartList;
			
		List<int> partCodes = new List<int>();
		
		for (int i = 0; i < partDatas.Length; ++i)
		{
			if (partDatas[i] != null)
			{
				partCodes.Add(partDatas[i].partCode);
			}
		}
		
		SetPartData(partCodes);
	}

	private void SetPartData(List<int> partCodes)
	{
		for (int i = 0; i < partCodes.Count; ++i)
		{
			passiveButton[i].SetButtonData(partCodes[i], i);
		}
	}

	private void SelectButton(int partCode, int selectIndex)
	{
		var emptyPart = PartEquipSystem.IsPartEmpty(selectIndex);

		selectPartCode = partCode;
		selectButtonIndex = selectIndex;

		if (!isSelect)
		{
			return;
		}

		// 비어있을 경우 즉시 착용
		if (emptyPart)
		{
			ChangePart(false);
		}
		else
		{
			//비어있지 않을 경우를 대비한 Modal.
			SelectModal.onClose?.AddListener(ChangePart);
			UIManager.Instance.OpenWindow(WindowList.PART_EQUIP_SELECT);
		}
	}

	private void ChangePart(bool isNo)
	{
		if (isNo)
		{
			PartEquipSystem.EquipPart(selectButtonIndex, selectPartCode, true);
			UpdatePartData();
			
			isSelect = false;
		}
		
		UIManager.Instance.RefreshWindow(WindowList.PART_EQUIP);
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
}