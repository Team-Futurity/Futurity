using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAbility : MonoBehaviour
{
	[SerializeField]
	private List<StatusData> status;

	private void Awake()
	{
		if(status.Count == 0)
		{
			FDebug.Log("Status Data?? ???????? ??????.", GetType());
		}
	}

	public List<StatusData> GetSubAbilityData()
	{
		return status;
	}
}
