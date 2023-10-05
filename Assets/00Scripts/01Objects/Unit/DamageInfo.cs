using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
	public readonly UnitBase Attacker;
	public readonly UnitBase Defender;
	public readonly float AttackST;
	public readonly ObjectPoolManager<Transform> HitEffect;

	public DamageInfo(UnitBase attacker, UnitBase defender, float attackST, ObjectPoolManager<Transform> hitEffect = null)
	{
		Attacker = attacker;
		Defender = defender;
		AttackST = attackST;
		HitEffect = hitEffect;
	}
}
