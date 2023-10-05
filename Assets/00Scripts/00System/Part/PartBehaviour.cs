using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehaviour : MonoBehaviour
{
	// Activation Percent
	public int partCode = 0;
	public PartType partType = PartType.NONE;
	
	// Player�� �������� Sub�� Core�� ����� �� �ֵ��� �Ѵ�.
	private SubAbility subAbility;
	private CoreAbility coreAbility;

	private bool isActive = false;

	private void Awake()
	{
		TryGetComponent(out coreAbility);

		if (partType == PartType.PASSIVE)
		{
			TryGetComponent(out subAbility);
		}

		isActive = false;
	}

	public void SetPartActive(bool isOn)
	{
		isActive = isOn;
	}

	public bool GetPartActive()
	{
		return isActive;
	}
	
	public List<StatusData> GetSubAbility()
	{
		if (!isActive)
		{
			return null;
		}

		return subAbility.GetSubAbilityData();
	}
	
	public void AddCoreAbilityToAttackEvent(DamageInfo info)
	{
		GetPassiveCoreAbility(info.Defender.GetComponent<UnitBase>());
	}

	public void ExecuteActiveAbility()
	{
		
	}

	private void GetPassiveCoreAbility(UnitBase enemy)
	{
		coreAbility.ExcutePart(enemy);
	}
}
