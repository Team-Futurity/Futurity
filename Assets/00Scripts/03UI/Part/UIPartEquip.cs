using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

	public TextMeshProUGUI activeText;

	[HideInInspector]
	public UnityAction<Animator> onEnded;
	private Animator playerAnim;

	private readonly string[] activeInfoTexts = new string[2]
	{
		"크로마 에너지를 구 형태로 차지한 뒤\n전방에 거대한 에너지 돔 폭발을 일으킨다.",
		"2회 발차기 공격을 통해 크로마 에너지를\n증폭시킨 뒤, 전방을 향해 거대한 회오리\n형태의 에너지를 발산한다."
	};

	public void OnEnable()
	{
		var playerPrefab = GameObject.Find("PlayerPrefab");
		playerAnim = playerPrefab.GetComponentInChildren<Animator>();
	}

	public void OnDisable()
	{
		if (canvasList == null && !UIManager.Instance.IsOpenWindow(WindowList.PART_EQUIP))
			return;

		foreach (var canvas in canvasList)
		{
			canvas.SetActive(true);
		}

		isSelect = false;
	}

	public void SetSelectPart(int code)
	{
		if (isSelect)
			return;
		
		selectPartCode = code;
		isSelect = true;
		partType = code > 2200 ? 1 : 2;

		if (partType == 1)
		{
			// Dim 
			// Active
			lockPassive.SetActive(true);
			lockActive.SetActive(false);

			if (activetype == 1)
			{
				activeIconImage.sprite = selectBasicActive;
			}
			else if (activetype == 2)
			{
				activeIconImage.sprite = selectBetaActive;
			}
		}
		else
		{
			lockActive.SetActive(true);
			lockPassive.SetActive(false);
			
			// Passive
			UIInputManager.Instance.SetUnableMoveButton(false);
			UIInputManager.Instance.SetMaxMoveIndex(3);

			active3.color = active2.color = active1.color = Define.noneSelectColor;
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
		
		if (PartSystem.IsIndexPartEmpty(2)) { passiveButton[0].InitResource(); }
		else { passiveButton[0].SetButtonData(passivePartDatas[2].partCode); }
		if (PartSystem.IsIndexPartEmpty(1)) { passiveButton[1].InitResource(); }
		else { passiveButton[1].SetButtonData(passivePartDatas[1].partCode); }
		if (PartSystem.IsIndexPartEmpty(0)) { passiveButton[2].InitResource(); }
		else { passiveButton[2].SetButtonData(passivePartDatas[0].partCode); }

		// Active Sync
		// 현재 PartSystem의 데이터와 Active UI를 맞추는 작업이 필요로 함.
		if (activePartData == 2201) { activeIconImage.sprite = selectBasicActive;
			activetype = 1; activeText.text = activeInfoTexts[0]; }
		else if (activePartData == 2202) { activeIconImage.sprite = selectBetaActive; activetype = 2; activeText.text = activeInfoTexts[1]; }
		else activetype = 0;
		
		activeButton.InitResource(true);
		activeButton.SetButtonData(activePartData);
	}

	// 버튼을 눌렀다는 것은 해당 Index에 부품을 장착하겠다는 소리임.
	private void SelectButton(int partCode, int selectIndex)
	{
		if (selectIndex == 999)
		{
			selectButtonIndex = selectIndex;
			UIInputManager.Instance.SaveIndex();
			
			SelectModal.SetNormalMode();
			
			UIManager.Instance.OpenWindow(WindowList.PART_EQUIP_SELECT);
			SelectModal.onClose?.AddListener(EquipSelectPart);

			return;
		}
		
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
			SelectModal.SetNormalMode();
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
			onEnded?.Invoke(playerAnim);
		}
		else // 다시 선택할 경우
		{
			UIManager.Instance.RefreshWindow(WindowList.PART_EQUIP);
			UIInputManager.Instance.SetSaveIndexToCurrentIndex();
			
			if(selectPartCode == 2201 || selectPartCode == 2202)
				UIInputManager.Instance.SetUnableMoveButton(true);
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