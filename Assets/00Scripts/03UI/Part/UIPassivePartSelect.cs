using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Passive Part Select System
// �нú� ��ǰ�� �����ϴ� System��.
public class UIPassivePartSelect : MonoBehaviour
{
	// ������ �ִ� Passive Button ��
	public List<UIPartSelectButton> partSelectButtons;
	
	// UI Part Equip System���� �ʿ��� ������ �����Ѵ�.
	[field: SerializeField, Space(15)]
	public UIPartEquip equip { get; private set; }

	private void Awake()
	{
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].onActive?.AddListener(OpenEquipWindow);
		}
	}

	// �ش� �޼��带  ���ؼ� Passive Select Part ����.
	public void SetPartData(int[] partCodes)
	{
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].SetButtonData(partCodes[i], i);
		}
	}

	// Code : ������ Button�� ������ �ִ� Part Code
	// Index : ������ Button�� ������ �ִ� Index -> ���⿡�� ������ ����.
	private void OpenEquipWindow(int code, int index)
	{
		// Equip System�� ���� ���� �Ѱ��ֱ�
		equip.SetSelectPart(code);
		
		// ��� Button�� Event ����
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].onActive.RemoveAllListeners();
		}

		// �ݱ� -> Passive Part Window
		UIManager.Instance.CloseWindow(WindowList.PASSIVE_PART);
		
		// ���� -> Part Equip Window
		UIManager.Instance.OpenWindow(WindowList.PART_EQUIP);
	}

}
