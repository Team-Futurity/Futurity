using System;
using System.Collections.Generic;
using UnityEngine;

public enum BossState : int
{
	SetUp,

	Idle,
	Chase,
	Hit,
	Death,

	T0_Dash,
	T1_Melee,
	T2_Ranged,
	T3_Move,
	T3_Laser,
	T4_Laser,
	T5_EnemySpawn,
	T6_Circle,
}

public enum Phase
{
	Phase1,
	Phase2,
}

[Serializable]
public class BossAttackData
{
	public Phase phase;
	public BossState state;

	public float extraAttackPoint;
	public float targetKnockbackPower;
	public int percentage;

	public float attackDelay;
	public float attackSpeed;
	public float attackAfterDelay;
}

[Serializable]
public class BossPhaseData
{
	public Phase phase;
	public int hpPercentage;
}
