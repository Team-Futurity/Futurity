using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Common



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

public enum PassivePartAbility
{
	NONE = 0,

	ATTACKEFFECT_HEALTH = 11,
	ATTACKEFFECT_SHOCK,

	CHANGED_DEFENCE = 21,
	CHANGED_CRITICAL_CHANCE,
	CHANGED_ATTACK,

	MAX = 99
}


// Active

