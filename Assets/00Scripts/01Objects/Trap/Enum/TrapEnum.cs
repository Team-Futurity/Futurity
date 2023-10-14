using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrapCondition
{
	NONE = 0,
	
	IN,
	ATTACK,
	
	MAX
}

public enum TrapType
{
	NONE = 0,
	
	FALL,
	Debuff,
	Explosion,
	
	MAX
}