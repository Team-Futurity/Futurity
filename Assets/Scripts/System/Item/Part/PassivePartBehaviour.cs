using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePartBehaviour : PartBehaviour
{
	[field: SerializeField] public PassivePartItemData PassivePartData { get; private set; }

	// Buff
	[field: SerializeField] public BuffSystem buffSystem { get; private set; }
	[field: SerializeField] public BuffNameList buffName { get; private set; }

	// Status
	[field: SerializeField] public StatusManager statusManager { get; private set; }

	private void Awake()
	{
		SetData(PassivePartData);
	}

	public override void OnAction(UnitBase unit)
	{
		buffSystem?.OnBuff(buffName, unit);

		if(statusManager is not null)
		{
			unit.status.AddStatus(statusManager.GetStatus());
		}
	}
}
