using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		switch (part.PartItemData.PartType)
		{
			case PartTriggerType.PASSIVE:
				PassiveEquip(part);
				break;

			case PartTriggerType.ACTIVE:
				ActiveEquip(part);
				break;

			default:
				break;
		}
	}

	public void UnequipPart(Part part)
	{
		if(equipPart.Count <= 0)
		{
			return;
		}

		switch(part.PartItemData.PartType)
		{
			case PartTriggerType.PASSIVE:
				PassiveUnequip(part);
				break;

			case PartTriggerType.ACTIVE:
				ActiveUnequip(part);
				break;

			default:
				break;
		}

		equipPart.Remove(part);

	}

	private void PassiveEquip(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if (passivePart is null)
		{
			FDebug.Log($"{part.GetType()}이(가) 존재하지 않습니다.");
			return;
		}

		// 패시브 타입은 버프 혹은 스탯을 반환한다.
		var data = passivePart.GetData();

		manager.AddStatus(data.status);

		ownerUnit.status.AddStatus(data.status);
	}

	private void ActiveEquip(Part part)
	{

	}


	private void PassiveUnequip(Part part)
	{
		part.TryGetComponent(out IPassive passivePart);

		if(passivePart is null)
		{
			FDebug.Log($"{part.GetType()}이(가) 존재하지 않습니다.");
			return;
		}

		var data = passivePart.GetData();

		manager.SubStatus(data.status);

		ownerUnit.status.SubStatus(data.status);
	}

	private void ActiveUnequip(Part part)
	{

	}
}
