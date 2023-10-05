using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo �ý���")]
	private ComboGaugeSystem comboGaugeSystem;

	// 0 ~ 2	: Passive
	// 3		: Active
	[SerializeField, Header("���� ��ǰ ���")]
	private List<PartBehaviour> equipPartList = new List<PartBehaviour>();

	[SerializeField, Header("����� ��")]
	private List<StatusData> status;

	public float debugPercent = .0f;
	
	private void Awake()
	{
		ClearStatus();
	}

	private void Update()
	{
		// Percent�� ���� ���� �۵� ���� Ȯ��
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			UpdateComboGauge(debugPercent);
		}

		// ���� ��ų �۵� ���� Ȯ��
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			// equipPartList[3].AddCoreAbilityToAttackEvent(enemy);
		}
	}

	public void EquipPart()
	{
		
	}

	public void UnEquipPart()
	{

	}

	// Combo System���� Update �߰��ϱ�.
	private void UpdateComboGauge(float percent)
	{
		int activePossibleCount = (int)Math.Floor(percent / 25f);
		int maxPartCount = equipPartList.Count - 1;

		// Active
		for (int i = 0; i < ((activePossibleCount > maxPartCount) ? maxPartCount : activePossibleCount); ++i)
		{
			ExecuteParts(i);
		}

		// UnActive
		for (int i = maxPartCount; i >= activePossibleCount; --i)
		{
			StopParts(i);
		}
	}

	private void ExecuteParts(int index)
	{
		var part = equipPartList[index];

		if (!part.GetPartActive())
		{
			part.SetPartActive(true);

			AddStatus(part.GetSubAbility());

			if (index == 2)
			{
				Debug.Log(index + " : CORE ACTIVE");
			}
		}
	}

	private void StopParts(int index)
	{
		var part = equipPartList[index];

		if (part.GetPartActive())
		{
			SubStatus(part.GetSubAbility());

			if(index == 2)
			{
				Debug.Log(index + " : CORE UN ACTIVE ");
			}

			part.SetPartActive(false);
		}
	}
	
	#region Status Feature

	private void AddStatus(List<StatusData> statusData)
	{
		foreach (var statusElement in statusData)
		{
			var element = status.Find((x) => x.type == statusElement.type);
			var hasStatus = (element is null);

			if (hasStatus)
			{
				status.Add(statusElement);
			}
			else
			{
				element.AddValue(statusElement.GetValue());
			}
		}
	}

	private void SubStatus(List<StatusData> statusData)
	{
		foreach (var statusElement in statusData)
		{
			var element = status.Find((x) => x.type == statusElement.type);
			element.SubValue(statusElement.GetValue());
		}
	}

	private void ClearStatus()
	{
		status.Clear();
	}
	
	#endregion
}