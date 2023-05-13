using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TrapData
{
	// Trap Code - Not Unique
	public int code;

	// Trap Code - Unique 
	public string uID;

	// String
	public string name;

	// Int
	public int condition;
	public int type;
	public int debuff;

	// Float Data
	public float range;
	public float damage;
	public float duration;
	public float cooldowns;
}