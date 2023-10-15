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

	// 0 ~ 2	: Passive
	// 3		: Active
	public PartBehaviour[] equipPartList = new PartBehaviour[4];

	[SerializeField, Header("디버그 용")]
	private List<StatusData> status;

	public float debugPercent = .0f;

	private const int UseCoreAbility = 2;
	private const int ActivePartIndex = 3;

	// Part가 장착되었을 때. 혹은 장착 해제 되었을 때.
	// Index, PartCode
	[HideInInspector] public UnityEvent<int, int> onPartEquip;
	[HideInInspector] public UnityEvent<int, int> onPartUnEquip;

	// Part가 활성화, 비활성화 되었을 때.
	// PartCode
	[HideInInspector] public UnityEvent<int> onPartActive;
	[HideInInspector] public UnityEvent<int> onPartDeactive;
	
	private void Awake()
	{
		ClearStatus();

		TryGetComponent(out player);
		
		comboGaugeSystem.OnGaugeChanged?.AddListener(UpdateComboGauge);
	}

	public void EquipPart(int index, int partCode, bool isForced = false)
	{
		if (equipPartList[index] != null && !isForced)
		{
			if (equipPartList[index].partCode != 0)
			{
				FDebug.Log($"해당하는 Index에 이미 Part가 존재합니다.");
				return;
			}
		}
		
		var part = PartDatabase.GetPart(partCode);
		equipPartList[index] = part;
		// Index, Code
		onPartEquip?.Invoke(index, partCode);
		FDebug.Log($"{index +1}번째에 {partCode}에 해당하는 파츠 장착 완료", part.GetType());
	}
	
	public void UnEquipPart(int index)
	{
		var partCode = equipPartList[index].partCode;
		onPartUnEquip?.Invoke(index, partCode);

		equipPartList[index] = null;
		FDebug.Log($"{index +1}번째에 해당하는 파츠 장착 해제");
	}

	// Select index : 현재 선택된 Index
	// Change Index : 교체를 희망하고 있는 Index
	public void SwapPart(int selectIndex, int changeIndex)
	{
		(equipPartList[selectIndex], equipPartList[changeIndex]) = (equipPartList[changeIndex], equipPartList[selectIndex]);
	}

	public bool IsPartEmpty(int index)
	{
		return (equipPartList[index] == null);
	}


	#region Part Activate
	private void UpdateComboGauge(float percent, float max)
	{
		int activePossibleCount = (int)Math.Floor(percent / 25f);
		int maxPartCount = equipPartList.Length - 1;

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
		
		if (part == null)
		{
			return;
		}

		if (!part.GetPartActive())
		{
			part.SetPartActive(true);
			
			AddStatus(part.GetSubAbility());

			if (index == UseCoreAbility)
			{
				player.onAttackEvent?.AddListener(part.AddCoreAbilityToAttackEvent);
			}

			if (index == ActivePartIndex)
			{
				
			}

			var partCode = part.partCode;

			onPartEquip?.Invoke(partCode);
		}
	}

	private void StopParts(int index)
	{
		var part = equipPartList[index];

		if (part.GetPartActive())
		{
			SubStatus(part.GetSubAbility());

			if(index == UseCoreAbility)
			{
				player.onAttackEvent?.RemoveListener(part.AddCoreAbilityToAttackEvent);
			}

			if (index == ActivePartIndex)
			{
				
			}

			part.SetPartActive(false);

			var partCode = part.partCode;

			onPartUnEquip?.Invoke(partCode);
		}
	}
	#endregion

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