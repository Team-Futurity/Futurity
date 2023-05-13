using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedTrap : TrapBehaviour
{
	private void Start()
	{
		resetEvent.AddListener(ResetEvent);
	}

	// Trap이 발동되었을 때
	protected override void ActiveTrap()
	{
		FDebug.Log("감전이 발생되었습니다.");
	}

	private void ResetEvent()
	{
		FDebug.Log("리셋이 완료되었습니다.");
	}
}
