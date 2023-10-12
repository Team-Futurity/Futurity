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
	
	// Select Modal 창

	private int selectPartCode = 0;

	private void Awake()
	{
		for (int i = 0; i < passiveButton.Count; ++i)
		{
			passiveButton[i].onActive?.AddListener(SelectButton);
		}
	}

	private void Start()
	{
		UpdatePartData();
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

	public void SetSelectPart(int code)
	{
		selectPartCode = code;
	}

	private void SetPartData(List<int> partCodes)
	{
		for (int i = 0; i < partCodes.Count; ++i)
		{
			passiveButton[i].SetButtonData(partCodes[i], i);
		}
	}

	private void SelectButton(int partCode, int index)
	{
		// 선택되어 있는 파츠가 존재하지 않음
		if (partCode == 0)
		{
			PartEquipSystem.EquipPart(index, partCode);
			UpdatePartData();
		}
		else
		{
			// 존재할 경우, 모달창 띄우기
		}
	}
}