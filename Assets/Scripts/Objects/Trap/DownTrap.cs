using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTrap : TrapBehaviour
{
	// 하강 리소스 오브젝트
	[SerializeField] private DownTrapFallObject fallObj;

	private void Awake()
	{
		if(fallObj is null)
		{
			FDebug.Log($"{fallObj.GetType()}이 존재하지 않습니다.");
		}
	}

	protected override void ActiveTrap()
	{
		// 하강 리소스가 플레이어와 충돌하면 데미지 입력.
	}

}
