using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Common

public enum PartTriggerType
{
	NONE = 0,

	PASSIVE,
	ACTIVE,

	MAX = 99
}


// Passive

public enum PassivePartGrade
{
	NONE = 0,

	DEFAULT,
	C,
	B,
	A,

	MAX = 99
}

public enum PassiveApplyType
{
	NONE = 0,

	ATTACK,
	STATUS,

	MAX = 99
}

public struct PassiveData
{
	public PassiveData(List<StatusData> status, BuffName buffName)
	{
		this.status = status;
		this.buffName = buffName;
	}

	public List<StatusData> status;
	public BuffName buffName;
}

// Active

