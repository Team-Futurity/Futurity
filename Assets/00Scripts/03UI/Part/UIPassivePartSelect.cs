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

	// �ش� �޼��带  ���ؼ� Passive Select Part ����.
	public void SetPartData(int[] partCodes)
	{
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].SetButtonData(partCodes[i], i);
		}
	}

	private void OpenEquipWindow(int code, int index)
	{
		// Equip System�� ���� ���� �Ѱ��ֱ�
		equip.SetSelectPart(code);
		
		for (int i = 0; i < partSelectButtons.Count; ++i)
		{
			partSelectButtons[i].onActive.RemoveAllListeners();
		}

		// �ݱ�
		UIManager.Instance.CloseWindow(WindowList.PASSIVE_PART);
		
		// ����
		UIManager.Instance.OpenWindow(WindowList.PART_EQUIP);

		
	}

}
