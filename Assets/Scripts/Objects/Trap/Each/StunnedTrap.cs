using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunnedTrap : TrapBehaviour
{
	private BuffSystem buffSystem;

	private void Awake()
	{
		TryGetComponent(out buffSystem);
	}
	
	public override void ActiveTrap(List<UnitBase> units)
	{
		foreach (var unit in units) 
		{
			// buffSystem.OnBuff(BuffNameList.SHOCK, unit);
		}
	}
}
