using IngameDebugConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PartSystem : MonoBehaviour
{
	[SerializeField, Header("Combo �ý���")]
	private ComboGaugeSystem comboGaugeSystem;

	private Player player;

	// 0 ~ 2	: Passive
	// 3		: Active
	public static PartBehaviour[] equipPartList = new PartBehaviour[4];

	[SerializeField, Header("����� ��")]
	private List<StatusData> status;

	public float debugPercent = .0f;

	private const int UseCoreAbility = 2;
	private const int ActivePartIndex = 3;
	
	private void Awake()
	{
		ClearStatus();

		TryGetComponent(out player);
		
		// �޺� ������ �߰�
		
		// �÷��̾� ���� �߰�
		//player.onAttackEvent.AddListener();

		Debug.Log(equipPartList.Length);
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

	[ConsoleMethod("EquipPart", "Equip Part")]
	public static void EquipPart(int index, int partCode)
	{
		if (equipPartList[index] != null)
		{
			FDebug.Log($"�ش��ϴ� Index�� �̹� Part�� �����մϴ�.");
			return;
		}
		
		var part = PartDatabase.GetPart(partCode);
		equipPartList[index] = part;
		
		FDebug.Log($"{index +1}��°�� {partCode}�� �ش��ϴ� ���� ���� �Ϸ�", part.GetType());
	}

	[ConsoleMethod("RemovePart", "Part Remove")]
	public void UnEquipPart(int index)
	{
		equipPartList[index] = null;
		FDebug.Log($"{index +1}��°�� �ش��ϴ� ���� ���� �Ϸ�");
	}

	// Select index : ���� ���õ� Index
	// Change Index : ��ü�� ����ϰ� �ִ� Index
	[ConsoleMethod("SwapPart", "Swap")]
	public static void SwapPart(int selectIndex, int changeIndex)
	{
		(equipPartList[selectIndex], equipPartList[changeIndex]) = (equipPartList[changeIndex], equipPartList[selectIndex]);
	}

	public bool IsPartValue(int index)
	{
		return (equipPartList[index] != null);
	}

	private void UpdateComboGauge(float percent)
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