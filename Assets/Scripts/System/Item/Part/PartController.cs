using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartController : MonoBehaviour
{
	[Header("���� ���� ����")]
	public List<Part> equipPart;
	private const int MaxEquipCount = 4;

	private PlayerController ownerUnit;

	public SkillIconTree socket;

	private StatusManager manager = new();
	private OriginStatus status;

	private float playerGauge = .0f;

	// Test�� �ڵ� <- Epic Monster�� �������� �ʾƼ� ��ǰ ������ ����
	[Header("�׽�Ʈ�� ����")]
	public List<Part> testPart;

	private void Awake()
	{
		status = ScriptableObject.CreateInstance<OriginStatus>();

		status.AutoGenerator();

		manager.SetStatus(status.GetStatus());

		TryGetComponent(out ownerUnit);
		
		ownerUnit.comboGaugeSystem.OnGaugeChanged.AddListener(OnGaugeChanged);

		foreach (var part in equipPart)
		{
			if (part.PartItemData.PartActivation >= 60)
			{
				socket.SetSocket(part.PartUIData, 2);
			}
			else if (part.PartItemData.PartActivation >= 40)
			{
				socket.SetSocket(part.PartUIData, 1);
			}
			else if (part.PartItemData.PartActivation >= 20)
			{
				socket.SetSocket(part.PartUIData, 0);
			}
		}
	}

	private void OnDisable()
	{
		foreach(var part in equipPart)
		{
			part.SetActive(false);
		}
	}

	public void EquipPart(Part part)
	{
		if (equipPart.Count >= MaxEquipCount)
		{
			return;
		}


		if (part.PartItemData.PartActivation <= playerGauge)
		{
			part.SetActive(true);
			RunPassive(part);
		}

		equipPart.Add(part);
	}

	public void UnequipPart(Part part)
	{
		if(equipPart.Count <= 0)
		{
			return;
		}

		if (part.PartItemData.PartType == PartTriggerType.PASSIVE)
		{
			StopPassive(part);
		}
		else
		{
			StopActive(part);
		}

		equipPart.Remove(part);
	}

	private void OnGaugeChanged(float gauge)
	{
		playerGauge = gauge;

		if (equipPart.Count <= 0)
		{
			return;
		}
		
		foreach(var part in equipPart)
		{
			var partActivation = part.PartItemData.PartActivation;

			if(gauge >= 100 && !part.GetActive() && part.PartItemData.PartType == PartTriggerType.ACTIVE)
			{
				part.SetActive(true);
				RunActive(part);
				return;
			}
			else if(gauge < 100 && part.GetActive() && part.PartItemData.PartType == PartTriggerType.ACTIVE)
			{
				part.SetActive(false);
				StopActive(part);
				return;
			}

			if(partActivation <= gauge && !part.GetActive())
			{
				part.SetActive(true);

				RunPassive(part);
			}
			else if (partActivation > gauge && part.GetActive())
			{
				part.SetActive(false);
				
				StopPassive(part);
			}
		}
	}

	// Passive

	private void RunPassive(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if (passivePart is null)
		{
			FDebug.Log($"{part.GetType()}��(��) �������� �ʽ��ϴ�.");
			return;
		}

		var partData = passivePart.GetData();

		manager.AddStatus(partData.status);
		ownerUnit.playerData.status.AddStatus(partData.status);
	}

	private void StopPassive(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if(passivePart is null)
		{
			FDebug.Log($"{part.GetType()}��(��) �������� �ʽ��ϴ�.");
			return;
		}

		var partData = passivePart.GetData();

		manager.SubStatus(partData.status);
		ownerUnit.playerData.status.SubStatus(partData.status);
	}

	// Active

	private void RunActive(Part part)
	{
		part.TryGetComponent(out IActive activePart);

		if(activePart is null)
		{
			FDebug.Log($"{part.GetType()}��(��) �������� �ʽ��ϴ�.");
			return;
		}

		activePart.RunActive(ownerUnit);
	}

	private void StopActive(Part part)
	{
		part.TryGetComponent(out IActive activePart);

		if (activePart is null)
		{
			FDebug.Log($"{part.GetType()}��(��) �������� �ʽ��ϴ�.");
			return;
		}

		activePart.StopActive();
	}
}
