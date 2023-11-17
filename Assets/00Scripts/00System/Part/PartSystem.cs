using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Video;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo 시스템")]
	private ComboGaugeSystem comboGaugeSystem;

	[SerializeField, Header("Player Status")]
	private StatusManager status;

	private Player player;

	// Passive Part Variable
	[SerializeField, Header("패시브 파츠")]
	private PartBehaviour[] passiveParts = new PartBehaviour[3];
	private const int CORE_ACTIVE_INDEX = 2;
	private const int ACTIVE_PART_INDEX = 3;

	// Active Part Variable
	[SerializeField, Header("액티브 파츠")]
	public SpecialMoveType activePartType = SpecialMoveType.Basic;
	public bool isStartActivePart = false; 
		
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
		calcStatus = new List<StatusData>();

		TryGetComponent(out player);

		comboGaugeSystem.OnGaugeChanged?.AddListener(UpdatePartActivate);
	}

	private void Start()
	{
		LoadPartData();
	}

	private void OnApplicationQuit()
	{
		ClearPartData();
	}

	private void LoadPartData()
	{
		// Passive Part
		for (int i = 0; i < 3; ++i)
		{
			var data = PlayerPrefs.GetInt($"PassivePart{i}");

			if (data == 0)
			{
				continue;
			}
			
			EquipPassivePart(i, data);
		}

		var active = PlayerPrefs.GetInt("ActivePart");
		EquipActivePart(active);
	}

	private void ClearPartData()
	{
		for (int i = 0; i < 3; ++i)
		{
			PlayerPrefs.SetInt($"PassivePart{i}", 0);
		}
		PlayerPrefs.SetInt("ActivePart", 0);
	}

	public PartBehaviour[] GetPassiveParts()
	{
		return passiveParts;
	}

	public int GetActivePartCode()
	{
		return (int)activePartType;
	}

	#region Equip & UnEquip

	public void EquipPassivePart(int index, int partCode)
	{
		if (partCode == 0) return;
		if (PartDatabase.HasPartCode(partCode) == false)
		{
			FDebug.Log($"{partCode}에 해당하는 Part가 존재하지 않습니다.", GetType());
			return;
		}

		if (index == 2)
		{
			player.onAttackEvent?.RemoveAllListeners();
		}

		if (passiveParts[index] != null)
		{
			if (passiveParts[index].GetPartActive())
			{
				status.SubStatus(calcStatus);
				SubStatus(passiveParts[index].GetSubAbility());
			}
			passiveParts[index].SetPartActive(false);
		}

		passiveParts[index] = PartDatabase.GetPart(partCode);
		PlayerPrefs.SetInt($"PassivePart{index}", partCode);

		onPartEquip?.Invoke(index, partCode);

		FDebug.Log($"{index + 1}번째에 {partCode}에 해당하는 파츠 장착 완료", GetType());
		UpdatePartActivate(comboGaugeSystem.CurrentGauge, comboGaugeSystem.maxComboGauge);
	}

	public void EquipActivePart(int partCode)
	{
		partCode = partCode == 2202 ? (int)SpecialMoveType.Beta : (int)SpecialMoveType.Basic;
		activePartType = (SpecialMoveType)partCode;

		PlayerPrefs.SetInt("ActivePart", partCode);
		
		onPartEquip?.Invoke(999, partCode);
		UpdatePartActivate(comboGaugeSystem.CurrentGauge, comboGaugeSystem.maxComboGauge);
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
		// 존재하지 않는 Part Return
		if (IsIndexPartEmpty(index - 1)) return;
		if (index == 4)
		{
			if(isStartActivePart) return;
		}
		else if (IsIndexPartActivate(index - 1)) return;

		if(index == ACTIVE_PART_INDEX + 1)
		{
			isStartActivePart = true;
		}
		else
		{
			var passivePart = passiveParts[index - 1];
			passivePart.SetPartActive(true);

			// 1. Sub Ability
			AddStatus(passivePart.GetSubAbility());

			// 2. Core Ability
			if(index == CORE_ACTIVE_INDEX + 1)
			{
				player.onAttackEvent?.AddListener(passivePart.AddCoreAbilityToAttackEvent);
			}
			
			status.AddStatus(calcStatus);
		}
		
		onPartActive?.Invoke(index - 1);
	}

	private void DeactivatePart(int index)
	{
		if (IsIndexPartEmpty(index - 1)) return;
		if (index == 4)
		{
			if(!isStartActivePart) return;
		}
		else if (!IsIndexPartActivate(index - 1)) return;

		if (index == ACTIVE_PART_INDEX + 1)
		{
			isStartActivePart = false;
		}
		else
		{
			var passivePart = passiveParts[index - 1];

			// 1. Sub Ability
			status.SubStatus(calcStatus);
			SubStatus(passivePart.GetSubAbility());

			// 2. Core Ability
			if (index == CORE_ACTIVE_INDEX)
			{
				player.onAttackEvent?.RemoveListener(passivePart.AddCoreAbilityToAttackEvent);
			}

			passivePart.SetPartActive(false);
		}

		onPartDeactive?.Invoke(index - 1);
	}

	public bool IsIndexPartEmpty(int index)
	{
		if (index == 3) { return false;}
		return (passiveParts[index] == null);
	}

	// Part가 실행중인가?
	private bool IsIndexPartActivate(int index)
	{
		if (index == ACTIVE_PART_INDEX)
		{
			return activePartType == SpecialMoveType.None;
		}
		else
		{
			return passiveParts[index].GetPartActive();
		}
	}
	
	#endregion

	public int GetEquiped75PercentPointPartCode()
	{
		if (passiveParts[2] == null || !passiveParts[2].GetPartActive()) { return 404; }

		return passiveParts[2].partCode;
	}
	
	private int GetEquipPassivePartCode(int index)
	{
		return passiveParts[index].partCode;
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