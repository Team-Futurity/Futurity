using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PartController : MonoBehaviour
{
	public List<Part> equipPart = new List<Part>();
	public int equipCount = 0;

	// Owner Data
	private UnitBase ownerUnit;

	private StatusManager manager = new StatusManager();
	
	private OriginStatus status;

	private void Awake()
	{
		status = ScriptableObject.CreateInstance<OriginStatus>();
		status.AutoGenerator();

		manager.SetStatus(status.GetStatus());
		// On Gauge Event Add
	}

	private void Start()
	{
		//PassiveEquip(equipPart[0]);
	}

	public void SetOwnerUnit(UnitBase unit)
	{
		if (unit is not null)
		{
			ownerUnit = unit;
		}
	}

	public void EquipPart(Part part)
	{
		if (equipPart.Count >= equipCount)
		{
			return;
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
		foreach(var part in equipPart)
		{
			var partActivation = part.PartItemData.PartActivation;

			if(partActivation <= gauge && !part.GetActive())
			{
				part.SetActive(true);

				// Passive
				RunPassive(part);
			}
		}
	}

	// Passive

	private void RunPassive(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if (passivePart is null)
		{
			FDebug.Log($"{part.GetType()}이(가) 존재하지 않습니다.");
			return;
		}

		var partData = passivePart.GetData();

		manager.AddStatus(partData.status);
		ownerUnit.status.AddStatus(partData.status);
	}

	private void StopPassive(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if(passivePart is null)
		{
			FDebug.Log($"{part.GetType()}이(가) 존재하지 않습니다.");
			return;
		}

		var partData = passivePart.GetData();

		manager.SubStatus(partData.status);
		ownerUnit.status.SubStatus(partData.status);
	}

	// Active

	private void RunActive(Part part)
	{

	}

	private void StopActive(Part part)
	{

	}
}
