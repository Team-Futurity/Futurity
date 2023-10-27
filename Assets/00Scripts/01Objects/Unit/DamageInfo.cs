using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInfo
{
	public readonly UnitBase Attacker;
	public readonly UnitBase Defender;
	public readonly float AttackST;
	public readonly float KnockbackPower;
	public float Damage { get; private set; }
	public ObjectPoolManager<Transform> HitEffectPoolManager { get; private set; }
	public Vector3 HitEffectOffset { get; private set; }
	public bool IsDot { get; private set; }
	public int StopFrameCount { get; private set; }

	public DamageInfo(UnitBase attacker, UnitBase defender, float attackST, float knockbackPower = 0)
	{
		Attacker = attacker;
		Defender = defender;
		AttackST = attackST;
		KnockbackPower = knockbackPower;
		StopFrameCount = 0;
	}

	public DamageInfo(DamageInfo origin)
	{
		Attacker = origin.Attacker;
		Defender = origin.Defender;
		AttackST = origin.AttackST;
		Damage = origin.Damage;
		HitEffectPoolManager = origin.HitEffectPoolManager;
		HitEffectOffset = origin.HitEffectOffset;
		IsDot = origin.IsDot;
		StopFrameCount = origin.StopFrameCount;
	}

	public void SetHitEffect(ObjectPoolManager<Transform> hitEffectPoolManager, Vector3? hitEffectOffset = null)
	{
		if(hitEffectPoolManager == null) { return; }

		HitEffectPoolManager = hitEffectPoolManager;
		HitEffectOffset = hitEffectOffset ?? Vector3.zero;
	}

	public void SetDamage(float damage)
	{
		Damage = damage;
	}

	public void SetIsDot(bool isDot)
	{
		IsDot = isDot;
	}

	public void SetStopFrameCount(int frameCount)
	{
		StopFrameCount = frameCount;
	}
}