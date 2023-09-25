using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
	public readonly UnitBase Attacker;
	public readonly UnitBase Defender;
	public readonly float AttackST;

	public DamageInfo(UnitBase attacker, UnitBase defender, float attackST)
	{
		Attacker = attacker;
		Defender = defender;
		AttackST = attackST;
	}
}
