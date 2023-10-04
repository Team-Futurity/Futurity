using System;
using UnityEngine;

public enum SkillType
{
	Type1_Melee,
	Type2_Ranged,
	Type3_Laser,
	Type4_Laser,
	Type5_EnemySpawn,
	Type6_Circle,
	Type7_Trap,
}

public enum Phase
{
	Phase1,
	Phase2,
	Phase3,
	Phase4,
}

[Serializable]
public class BossActiveData
{
	public Phase phase;
	public SkillType skillType;
	public float skillTypeDelay;
}

[Serializable]
public class BossPhaseData
{
	public Phase phase;
	public float percentage;
	public float type467MaxTime;
	public float type5MaxTime;
}
