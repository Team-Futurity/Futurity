using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedTrap : TrapBehaviour
{
	private void Start()
	{
		resetEvent.AddListener(ResetEvent);
	}

	// Trap�� �ߵ��Ǿ��� ��
	protected override void ActiveTrap()
	{
		FDebug.Log("������ �߻��Ǿ����ϴ�.");
	}

	private void ResetEvent()
	{
		FDebug.Log("������ �Ϸ�Ǿ����ϴ�.");
	}
}
