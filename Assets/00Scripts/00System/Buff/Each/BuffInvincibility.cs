using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInvincibility : BuffBehaviour
{
	// �����Լ� ���� ���°� �ȴ�.
	
	public override void Active()
	{
		base.Active();

		targetUnit.isGodMode = true;
	}

	public override void UnActive()
	{
		targetUnit.isGodMode = false;
		
		base.UnActive();
	}
}
