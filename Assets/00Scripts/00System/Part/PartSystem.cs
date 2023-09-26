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

	// Combo System���� Update �߰��ϱ�.
	private void UpdateComboGauge(float percent)
	{
		int activePossibleCount = (int)Math.Floor(percent / 25f);
		int maxPartCount = equipPartList.Count;

		// 25, 50�� Sub�� ����
		// 75�� Sub, Core ����

		// Active
		for (int i = 1; i <= ((activePossibleCount > maxPartCount) ? maxPartCount : activePossibleCount); ++i)
		{
			AddStatus(equipPartList[i - 1].GetSubAbility());
		}

		// UnActive
		for (int i = maxPartCount; i > activePossibleCount; --i)
		{

		}
	}

	private void AddStatus(List<StatusData> statusData)
	{
		foreach (var statusElement in statusData)
		{
			var hasStatus = (status.Find((x) => x.type == statusElement.type) is null);

			if (hasStatus)
			{
				var element = status.Find((x) => x.type == statusElement.type);
				element = statusElement;
			}
			else
			{
				Debug.Log("�� ����� ��?");
				status.Add(statusElement);
			}
		}
	}

	private void SubStatus(List<StatusData> statusData)
	{
		foreach (var statusElement in statusData)
		{

		}
	}

	private void ClearStatus()
	{
		status.Clear();
	}
}