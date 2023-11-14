using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPartEquip : MonoBehaviour
{
	// 0, 1, 2 -> Top, Middle, Bottom
	public List<UIPartSelectButton> passiveButton;

	public UIPartSelectButton activeButton;

	[field: SerializeField]
	public PartSystem PartSystem { get; private set; }

	[field: SerializeField]
	public UIPartEquipSelect SelectModal { get; private set; }

	private int selectPartCode = 0;
	private int selectButtonIndex = 0;

	private bool isSelect = false;

	private int partType = 0;

	public Image activeIconImage;
	public Sprite normalBasicActive;
	public Sprite selectBasicActive;
	public Sprite normalBetaActive;
	public Sprite selectBetaActive;

	private int activetype = 0;

	public List<GameObject> canvasList;

	public GameObject lockPassive;
	public GameObject lockActive;

	public Image active1;
	public Image active2;
	public Image active3;
	
	public void OnDisable()
	{
		if (canvasList == null && !UIManager.Instance.IsOpenWindow(WindowList.PART_EQUIP))
			return;

		foreach (var canvas in canvasList)
		{
			canvas.SetActive(true);
		}
	}

	public void SetSelectPart(int code)
	{
		selectPartCode = code;
		isSelect = true;
		partType = code > 2200 ? 1 : 2;

		if (partType == 1)
		{
			// Dim 
			// Active
			UIInputManager.Instance.SetDefaultFocusForced(3);
			UIInputManager.Instance.SetUnableMoveButton(true);
			
			lockPassive.SetActive(true);
			lockActive.SetActive(false);

			if (activetype == 1)
				activeIconImage.sprite = selectBasicActive;
			else if (activetype == 2)
				activeIconImage.sprite = selectBetaActive;
		}
		else
		{
			lockActive.SetActive(true);
			lockPassive.SetActive(false);
			
			// Passive
			UIInputManager.Instance.SetUnableMoveButton(false);
			UIInputManager.Instance.SetMaxMoveIndex(3);

			active3.color = active2.color = active1.color = Define.noneSelectcolor;
		}
	}

	// PartSystem과 현재 장착 UI의 싱크를 맞춘다.
	public void SyncPartDataToPartSystem()
	{
		EnableSelectEvent();

		var passivePartDatas = PartSystem.GetPassiveParts();
		var activePartData = PartSystem.GetActivePartCode();

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

		// Active Sync
		// 현재 PartSystem의 데이터와 Active UI를 맞추는 작업이 필요로 함.
		if (activePartData == 2201) { activeIconImage.sprite = normalBasicActive; activetype = 1; }
		else if (activePartData == 2202) {activeIconImage.sprite = normalBetaActive; activetype = 2; }
		else activetype = 0;
		
		activeButton.InitResource(true);
		activeButton.SetButtonData(activePartData);
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
			if (partType == 2)
				PartSystem.EquipPassivePart(selectButtonIndex, selectPartCode);
			else
				PartSystem.EquipActivePart(selectPartCode);

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
		if (partType == 2)
		{
			for (int i = 0; i < passiveButton.Count; ++i)
			{
				passiveButton[i].onSelected?.AddListener(SelectButton);
			}
		}
		else
		{
			activeButton.onSelected?.AddListener(SelectButton);
		}
	}

	private void DisableSelectEvent()
	{
		if (partType == 2)
		{
			for (int i = 0; i < passiveButton.Count; ++i)
			{
				passiveButton[i].onSelected?.RemoveAllListeners();
			}
		}
		else
		{
			activeButton.onSelected?.AddListener(SelectButton);
		}
	}
}