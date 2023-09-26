using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehaviour : MonoBehaviour
{
	// Gauge를 제어하는 것은 Part System에서 진행한다. -> Part 넣는 위치에 따라서 달라지기 때문

	// Activation Percent
	public int partCode = 0;
	public PartType partType = PartType.NONE;
	
	// Player의 개념으로 Sub와 Core를 사용할 수 있도록 한다.
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

	public void OnFeature()
	{

	}

	public void SetPartActive(bool isOn)
	{
		isActive = isOn;
	}

	public bool GetPartActive()
	{
		return isActive;
	}

	public void GetCoreAbility()
	{

	}

	public List<StatusData> GetSubAbility()
	{
		if (!isActive)
		{
			return null;
		}

		return subAbility.GetSubAbilityData();
	}
}
