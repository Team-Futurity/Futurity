using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePart : Part
{
	// Active Behaviour
	[field: SerializeField] public ActiveBehaviour Behaviour { get; private set; }

	private void Awake()
	{
		if (PartItemData.PartType != PartTriggerType.ACTIVE)
		{
			FDebug.Log($"{PartItemData.PartType}�� ���� �ʽ��ϴ�.");
			Debug.Break();
		}
	}

}
