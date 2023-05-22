using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartController : MonoBehaviour
{
	public List<Part> equipPart = new List<Part>();
	private const int MaxEquipCount = 4;

	// Owner Data
	private PlayerController ownerUnit;
	
	private StatusManager manager = new StatusManager();
	
	private OriginStatus status;

	private float playerGauge = .0f;

	private void Awake()
	{
		status = ScriptableObject.CreateInstance<OriginStatus>();
		status.AutoGenerator();

		manager.SetStatus(status.GetStatus());

		TryGetComponent(out ownerUnit);
	}

	private void Start()
	{
		//PassiveEquip(equipPart[0]);

	}

	public void EquipPart(Part part)
	{
		if (equipPart.Count >= MaxEquipCount)
		{
			return;
		}

		if (part.PartItemData.PartActivation > playerGauge)
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
		
		foreach(var part in equipPart)
		{
			var partActivation = part.PartItemData.PartActivation;

			if(partActivation >= gauge && !part.GetActive())
			{
				part.SetActive(true);

				RunPassive(part);
			}
			else if (partActivation < gauge && part.GetActive())
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

	}

	private void StopActive(Part part)
	{

	}
}
