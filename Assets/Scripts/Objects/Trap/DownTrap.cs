using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrap : TrapBehaviour
{
	// �ϰ� ���ҽ� ������Ʈ
	[SerializeField] private DownTrapFallObject fallObj;

	private void Awake()
	{
		if(fallObj is null)
		{
			FDebug.Log($"{fallObj.GetType()}�� �������� �ʽ��ϴ�.");
		}
	}

	protected override void ActiveTrap()
	{
		// �ϰ� ���ҽ��� �÷��̾�� �浹�ϸ� ������ �Է�.
	}

}
