using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPassivePartSelect : MonoBehaviour
{
	public List<UIPartSelectButton> partSelectButtons;
	
	[field: SerializeField, Space(15)]
	public UIPartEquip equip { get; private set; }

	private void Awake()
	{
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].onActive?.AddListener(OpenEquipWindow);
			Debug.Log("ADD");
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

	private void OpenEquipWindow(int code, int index)
	{
		// Equip System에 선택 파츠 넘겨주기
		equip.SetSelectPart(code);
		
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].onActive.RemoveAllListeners();
		}

		// 닫기
		UIManager.Instance.CloseWindow(WindowList.PASSIVE_PART);
		
		// 열기
		UIManager.Instance.OpenWindow(WindowList.PART_EQUIP);

		
	}

}
