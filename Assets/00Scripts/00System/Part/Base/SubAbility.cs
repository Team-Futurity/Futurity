using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAbility : MonoBehaviour
{
	private StatusManager statusManager;

	private void Awake()
	{
		var isGetStatusManager = TryGetComponent(out statusManager);

		if (!isGetStatusManager)
		{
			FDebug.Log("StatusManager�� �������� �ʽ��ϴ�.", GetType());
		}
	}

	public void Active()
	{
		
	}

	private void OnSubAbility()
	{
		
	}
}
