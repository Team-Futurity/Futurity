using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Passive Part Select System
// 패시브 부품을 선택하는 System임.
public class UIPassivePartSelect : MonoBehaviour
{
	// 가지고 있는 Passive Button 수
	public List<UIPartSelectButton> partSelectButtons;
	
	// UI Part Equip System에서 필요한 설정을 진행한다.
	[field: SerializeField, Space(15)]
	public UIPartEquip equip { get; private set; }

	private void Awake()
	{
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].onActive?.AddListener(OpenEquipWindow);
		}
	}

	// 해당 메서드를  통해서 Passive Select Part 정리.
	public void SetPartData(int[] partCodes)
	{
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].SetButtonData(partCodes[i], i);
		}
	}

	// Code : 선택한 Button이 가지고 있는 Part Code
	// Index : 선택한 Button이 가지고 있는 Index -> 여기에선 사용되지 않음.
	private void OpenEquipWindow(int code, int index)
	{
		// Equip System에 선택 파츠 넘겨주기
		equip.SetSelectPart(code);
		
		// 모든 Button에 Event 제거
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].onActive.RemoveAllListeners();
		}

		// 닫기 -> Passive Part Window
		UIManager.Instance.CloseWindow(WindowList.PASSIVE_PART);
		
		// 열기 -> Part Equip Window
		UIManager.Instance.OpenWindow(WindowList.PART_EQUIP);
	}

}
