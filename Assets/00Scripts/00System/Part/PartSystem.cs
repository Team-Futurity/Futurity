using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo 시스템")]
	private ComboGaugeSystem comboGaugeSystem;
	
	[SerializeField, Header("장착 부품 목록")]
	private List<PartBehaviour> equipPartList = new List<PartBehaviour>();
	
	[SerializeField, Header("디버그 용")]
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

	// Combo System에 부품 추가
	private void AddComboEventToPart()
	{
		
	}

	// Combo System에서 부품 제거
	private void RemoveComboEventToPart()
	{
		
	}
}