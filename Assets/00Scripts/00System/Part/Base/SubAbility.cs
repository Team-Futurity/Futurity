using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAbility : MonoBehaviour
{
	[SerializeField]
	private List<StatusData> status;
	[SerializeField]
	private GameObject partActiveEffect;
	private GameObject instantiatedEffect;

	private void Awake()
	{
		if(status.Count == 0)
		{
			FDebug.Log("Status Data?? ???????? ??????.", GetType());
		}

		instantiatedEffect = Instantiate(partActiveEffect, transform);
		instantiatedEffect.SetActive(false);
	}

	public List<StatusData> GetSubAbilityData()
	{
		return status;
	}

	public void SetActiveEffect(bool isActive)
	{
		instantiatedEffect.SetActive(isActive);
	}
}
