using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBehaviour : PartBehaviour
{
	[field: SerializeField] public StatusManager status { get; private set; }
	[field: SerializeField] public BuffSystem buffSystem { get; private set; }

	[field: SerializeField] public PassiveApplyType PartApplyType { get; private set; }

	private void Awake()
	{
		// Set Attack Type
	}

	public void Active()
	{

	}

	private void SetStatus()
	{

	}

	private void OnBuff()
	{

	}
}
