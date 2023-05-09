using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInvincibility : BuffBehaviour
{

	// 적에게서 무적 상태가 된다.
	
	public override void Active(UnitBase unit)
	{
		base.Active(unit);
		
		targetUnit.SetGodMode(true);
	}

	public override void UnActive()
	{
		targetUnit.SetGodMode(false);
		
		base.UnActive();
	}
}
