using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo �ý���")]
	private ComboGaugeSystem comboGaugeSystem;

	private Player player;

	// Passive Part Variable
	[SerializeField, Header("�нú� ����")]
	private PartBehaviour[] passiveParts = new PartBehaviour[3];
	private const int CORE_ACTIVE_INDEX = 3;
	private const int ACTIVE_PART_INDEX = 4;

	// Active Part Variable
	[SerializeField, Header("��Ƽ�� ����")]
	private PartBehaviour activePart = new PartBehaviour();

	// Part�� ���� Status
	private List<StatusData> calcStatus;

	#region UnityEvents
	// Part�� �����Ǿ��� ��. Ȥ�� ���� ���� �Ǿ��� ��.
	// Index, PartCode
	[HideInInspector] public UnityEvent<int, int> onPartEquip;

	// Part�� Ȱ��ȭ, ��Ȱ��ȭ �Ǿ��� ��.
	// PartCode
	[HideInInspector] public UnityEvent<int, int> onPartActive;
	[HideInInspector] public UnityEvent<int, int> onPartDeactive;
	#endregion

	private void Awake()
	{
		// [Create] - Status Instance 
		calcStatus = new List<StatusData>();

		TryGetComponent(out player);

		comboGaugeSystem.OnGaugeChanged?.AddListener(UpdatePartActivate);
	}

	#region Equip & UnEquip

	public void EquipPassivePart(int index, int partCode)
	{
		passiveParts[index] = PartDatabase.GetPart(partCode);

		onPartEquip?.Invoke(index, partCode);
		FDebug.Log($"{index + 1}��°�� {partCode}�� �ش��ϴ� ���� ���� �Ϸ�", GetType());
	}

	public void EquipActivePart(int partCode)
	{
		activePart = PartDatabase.GetPart(partCode);

		onPartEquip?.Invoke(999, partCode);
	}

	#endregion

	#region Part Activate

	private void UpdatePartActivate(float currentGauge, float maxGauge)
	{
		int activePartCount = (int)Math.Floor(currentGauge / 25f);

		for (int i = 1; i <= activePartCount; ++i)
		{
			ActivatePart(i);
		}

		for(int i = 4; i > activePartCount; --i)
		{
			DeactivatePart(i);
		}
	}

	private void ActivatePart(int index)
	{
		// �������� �ʴ� Part Return
		if (IsIndexPartEmpty(index - 1)) return;
		// �������� Part Return
		if (IsIndexPartActivate(index)) return;

		if(index == ACTIVE_PART_INDEX)
		{
			// Active
			FDebug.Log($"Active Part Ȱ��ȭ");
		}
		else
		{
			var passivePart = passiveParts[index - 1];
			passivePart.SetPartActive(true);

			// 1. Sub Ability
			AddStatus(passivePart.GetSubAbility());

			// 2. Core Ability
			if(index == CORE_ACTIVE_INDEX)
			{
				player.onAttackEvent?.AddListener(passivePart.AddCoreAbilityToAttackEvent);
			}
		}

		onPartActive?.Invoke(index - 1, passiveParts[index -1].partCode);
	}

	private void DeactivatePart(int index)
	{
		if (IsIndexPartEmpty(index - 1)) return;
		if (!IsIndexPartActivate(index)) return;

		if (index == ACTIVE_PART_INDEX)
		{
			FDebug.Log($"Active Part ��Ȱ��ȭ");
		}
		else
		{
			var passivePart = passiveParts[index - 1];

			// 1. Sub Ability
			SubStatus(passivePart.GetSubAbility());

			// 2. Core Ability
			if (index == CORE_ACTIVE_INDEX)
			{
				player.onAttackEvent?.RemoveListener(passivePart.AddCoreAbilityToAttackEvent);
			}

			passivePart.SetPartActive(false);
		}

		onPartDeactive?.Invoke(index - 1, passiveParts[index - 1].partCode);
	}

	public bool IsIndexPartEmpty(int index)
	{
		if(index == ACTIVE_PART_INDEX - 1)
		{
			return (activePart == null);
		}
		else
		{
			return (passiveParts[index] == null);
		}
	}

	// Part�� �������ΰ�?
	private bool IsIndexPartActivate(int index)
	{
		if (index == ACTIVE_PART_INDEX)
		{
			return activePart.GetPartActive();
		}
		else
		{
			return passiveParts[index - 1].GetPartActive();
		}
	}
	
	#endregion

	private int GetEquipPassivePartCode(int index)
	{
		return passiveParts[index].partCode;
	}

	private int GetEquipActivePartCode()
	{
		return activePart.partCode;
	}

	#region Status Feature

	private void AddStatus(List<StatusData> statusData)
	{
		foreach (var statusElement in statusData)
		{
			var element = calcStatus.Find((x) => x.type == statusElement.type);
			var hasStatus = (element is null);

			if (hasStatus)
			{
				calcStatus.Add(statusElement);
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
			var element = calcStatus.Find((x) => x.type == statusElement.type);
			element.SubValue(statusElement.GetValue());
		}
	}

	private void ClearStatus()
	{
		calcStatus.Clear();
	}
	
	#endregion
}