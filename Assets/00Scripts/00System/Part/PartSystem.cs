using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo �ý���")]
	private ComboGaugeSystem comboGaugeSystem;
	
	[SerializeField, Header("���� ��ǰ ���")]
	private List<PartBehaviour> equipPartList = new List<PartBehaviour>();
	
	[SerializeField, Header("����� ��")]
	private List<StatusData> status;

	private void Awake()
	{
		status.Clear();
	}

	public void EquipPart()
	{
		
	}

	public void UnEquipPart()
	{
		
	}

	// Combo System�� ��ǰ �߰�
	private void AddComboEventToPart()
	{
		
	}

	// Combo System���� ��ǰ ����
	private void RemoveComboEventToPart()
	{
		
	}
}