using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPartEquip : MonoBehaviour
{
	// 0, 1, 2 -> Top, Middle, Bottom
	public List<UIPartSelectButton> passiveButton;

	[field: SerializeField]
	public PartSystem PartSystem { get; private set; }

	[field: SerializeField]
	public UIPartEquipSelect SelectModal { get; private set; }

	private int selectPartCode = 0;
	private int selectButtonIndex = 0;

	private bool isSelect = false;

	public void SetSelectPart(int code)
	{
		selectPartCode = code;
		isSelect = true;
	}

	// PartSystem과 현재 장착 UI의 싱크를 맞춘다.
	public void SyncPartDataToPartSystem()
	{
		EnableSelectEvent();

		var passivePartDatas = PartSystem.GetPassiveParts();
		
		var activePartData = PartSystem.GetActivePart();

		if (passivePartDatas == null)
		{
			for (int i = 0; i < passiveButton.Count; ++i)
			{
				passiveButton[i].InitResource();
			}

			return;
		}
		
		// Passive Sync
		for (int i = 0; i < passivePartDatas.Length; ++i)
		{
			if (PartSystem.IsIndexPartEmpty(i))
			{
				passiveButton[i].InitResource();
			}
			else
			{
				passiveButton[i].SetButtonData(passivePartDatas[i].partCode);
			}
		}
	}

	// 버튼을 눌렀다는 것은 해당 Index에 부품을 장착하겠다는 소리임.
	private void SelectButton(int partCode, int selectIndex)
	{
		// 해당 인덱스의 파츠의 Empty 여부
		var emptyPart = PartSystem.IsIndexPartEmpty(selectIndex);

		// 선택된 Index를 확인한다.
		selectButtonIndex = selectIndex;

		// Button의 현재 UI를 저장한다.
		UIInputManager.Instance.SaveIndex();

		// 없을 경우
		if (emptyPart)
		{
			EquipSelectPart(true);
		}
		else
		{
			UIManager.Instance.OpenWindow(WindowList.PART_EQUIP_SELECT);
			SelectModal.onClose?.AddListener(EquipSelectPart);
		}
	}

	// Part Close 버튼 클릭 시 나타남.
	private void EquipSelectPart(bool isEquip)
	{
		// Listener 제거
		SelectModal.onClose?.RemoveAllListeners();

		// Yes
		if (isEquip)
		{
			PartSystem.EquipPassivePart(selectButtonIndex, selectPartCode);

			DisableSelectEvent();
			UIManager.Instance.CloseWindow(WindowList.PART_EQUIP);

		}
		else // 다시 선택할 경우
		{
			UIManager.Instance.RefreshWindow(WindowList.PART_EQUIP);
			UIInputManager.Instance.SetSaveIndexToCurrentIndex();
		}
	}

	private void EnableSelectEvent()
	{
		for (int i = 0; i < passiveButton.Count; ++i)
		{
			passiveButton[i].onSelected?.AddListener(SelectButton);
		}
	}

	private void DisableSelectEvent()
	{
		for (int i = 0; i < passiveButton.Count; ++i)
		{
			passiveButton[i].onSelected?.RemoveAllListeners();
		}
	}
}