using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartController : MonoBehaviour
{
	// List Part
	public List<Part> equipPart = new List<Part>();
	public int equipCount = 0;

	// Calc
	public OriginStatus status;

	public void Awake()
	{
		status = ScriptableObject.CreateInstance<OriginStatus>();
		status.AutoGenerator();

		//PassiveEquip(equipPart[0]);
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
		// Find -> Remove
		// UnEquip Process Start
	}

	private void PassiveEquip(Part part)
	{
		IPassive passivePart;

		part.TryGetComponent(out passivePart);

		if(passivePart is null)
		{
			FDebug.Log($"{part.GetType()}이(가) 존재하지 않습니다.");
			return;
		}

		passivePart.Active(status);
	}

	private void ActiveEquip(Part part)
	{

	}
}
