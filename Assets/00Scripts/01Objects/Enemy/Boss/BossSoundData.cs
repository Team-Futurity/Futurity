using System;
using FMODUnity;


public enum SoundType : int
{
	NONE = 0,

	SHIELD_EFFECT,
	ATTACK_EFFECT,
	ANIMATION,

	MAX
}

[Serializable]
public class BossSoundData
{
	public BossState state;
	public SoundType soundType;
	public EventReference soundReference;
}