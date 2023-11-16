using System;
using FMODUnity;


public enum SoundType
{
	EFFECT,
	ANIMATION,
}

[Serializable]
public class BossSoundData
{
	public BossState state;
	public SoundType soundType;
	public EventReference soundReference;
}