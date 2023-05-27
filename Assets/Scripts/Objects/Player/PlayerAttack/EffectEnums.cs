using System;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
	Ready,
	Move,
	InstanceAttack,
	AfterDoingAttack
}

public enum EffectTarget
{
	Caster,
	Target,
	Ground
}

[Serializable]
public class RushEffectData
{
	public EffectType effectType;
	public EffectTarget effectTarget;
	public List<GameObject> effectList;
}

public class RushLevelEffect
{
	public int currentLevel;
	public RushEffectData data;
	public GameObject effect;
	public int index;

	public RushLevelEffect(int currentLevel, RushEffectData data, GameObject effect, int index)
	{
		this.currentLevel = currentLevel;
		this.data = data;
		this.effect = effect;
		this.index = index;
	}
}

public class TrackingEffectData
{
	public GameObject effect;
	public Transform target;

	public TrackingEffectData(GameObject effect, Transform target)
	{
		this.effect = effect;
		this.target = target;
	}
}