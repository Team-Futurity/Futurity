using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo 시스템")]
	private ComboGaugeSystem comboGaugeSystem;

	// 0 ~ 2	: Passive
	// 3		: Active
	[SerializeField, Header("장착 부품 목록")]
	private List<PartBehaviour> equipPartList = new List<PartBehaviour>();

	[SerializeField, Header("디버그 용")]
	private List<StatusData> status;

	public float debugPercent = .0f;

	private void Awake()
	{
		ClearStatus();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			UpdateComboGauge(debugPercent);
		}
	}

	public void EquipPart()
	{

	}

	public void UnEquipPart()
	{

	}

	// Combo System에서 Update 추가하기.
	private void UpdateComboGauge(float percent)
	{
		int activePossibleCount = (int)Math.Floor(percent / 25f);
		int maxPartCount = equipPartList.Count;

		// 25, 50은 Sub만 실행
		// 75는 Sub, Core 실행

		// Active
		for (int i = 1; i <= ((activePossibleCount > maxPartCount) ? maxPartCount : activePossibleCount); ++i)
		{
			ActivePart(equipPartList[i - 1]);
		}

		// UnActive
		for (int i = maxPartCount; i > activePossibleCount; --i)
		{
			 UnActivePart(equipPartList[i - 1]);
		}
	}

	private void ActivePart(PartBehaviour part)
	{
		if (!part.GetPartActive())
		{
			part.SetPartActive(true);

			AddStatus(part.GetSubAbility());
		}
	}

	private void UnActivePart(PartBehaviour part)
	{
		if(part.GetPartActive())
		{
			SubStatus(part.GetSubAbility());
			part.SetPartActive(false);
		}
	}

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
}