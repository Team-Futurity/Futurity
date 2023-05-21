using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePart : Part
{
	// Passive Behaviour
	[field: SerializeField] public PassiveBehaviour Behaviour { get; private set; }

	private void Awake()
	{
		if(PartItemData.PartType != PartTriggerType.PASSIVE)
		{
			FDebug.Log($"{PartItemData.PartType}�� ���� �ʽ��ϴ�.");
			Debug.Break();
		}
	}





}
