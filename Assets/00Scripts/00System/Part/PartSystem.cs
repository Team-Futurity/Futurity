using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo 시스템")]
	private ComboGaugeSystem comboGaugeSystem;

	private Player player;

	// Passive Part Variable
	[SerializeField, Header("패시브 파츠")]
	private PartBehaviour[] passiveParts = new PartBehaviour[3];
	private const int CORE_ACTIVE_INDEX = 3;
	private const int ACTIVE_PART_INDEX = 4;

	// Active Part Variable
	[SerializeField, Header("액티브 파츠")]
	private PartBehaviour activePart = new PartBehaviour();

	// Part가 계산된 Status
	private List<StatusData> calcStatus;

	#region UnityEvents
	// Part가 장착되었을 때. 혹은 장착 해제 되었을 때.
	// Index, PartCode
	[HideInInspector] public UnityEvent<int, int> onPartEquip;

	// Part가 활성화, 비활성화 되었을 때.
	// PartCode
	[HideInInspector] public UnityEvent<int> onPartActive;
	[HideInInspector] public UnityEvent<int> onPartDeactive;
	#endregion

	private void Awake()
	{
		// [Create] - Status Instance 
		calcStatus = new List<StatusData>();

		TryGetComponent(out player);
	}

	#region Equip & UnEquip

	public void EquipPassivePart(int index, int partCode)
	{
		passiveParts[index] = PartDatabase.GetPart(partCode);

		onPartEquip?.Invoke(index, partCode);
		FDebug.Log($"{index + 1}번째에 {partCode}에 해당하는 파츠 장착 완료", GetType());
	}

	public void EquipActivePart(int partCode)
	{
		activePart = PartDatabase.GetPart(partCode);

		onPartEquip?.Invoke(999, partCode);
	}

	#endregion

	#region Part Activate

	public int debug = 0;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			UpdatePartActivate(debug, 1f);
		}
	}

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
		// 존재하지 않는 Part Return
		if (IsIndexPartEmpty(index)) return;
		// 실행중인 Part Return
		if (IsIndexPartActivate(index)) return;

		if(index == ACTIVE_PART_INDEX)
		{
			// Active
			FDebug.Log($"Active Part 활성화");
		}
		else
		{
			var passivePart = passiveParts[index - 1];

			// 1. Sub Ability
			AddStatus(passivePart.GetSubAbility());

			// 2. Core Ability
			if(index == CORE_ACTIVE_INDEX)
			{
				player.onAttackEvent?.AddListener(passivePart.AddCoreAbilityToAttackEvent);
			}

			passivePart.SetPartActive(true);
		}

		onPartActive?.Invoke(index);
	}

	private void DeactivatePart(int index)
	{
		if (IsIndexPartEmpty(index)) return;
		if (!IsIndexPartActivate(index)) return;

		if (index == ACTIVE_PART_INDEX)
		{
			FDebug.Log($"Active Part 비활성화");
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

		onPartDeactive?.Invoke(index);
	}

	private bool IsIndexPartEmpty(int index)
	{
		if(index == ACTIVE_PART_INDEX)
		{
			return (activePart == null);
		}
		else
		{
			return (passiveParts[index - 1] == null);
		}
	}

	// Part가 실행중인가?
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