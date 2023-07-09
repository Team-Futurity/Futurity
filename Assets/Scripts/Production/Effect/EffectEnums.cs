using System;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal.Internal;
using static PlayerController;

/*public enum EffectType
{
	None,
	Attack
}*/

public enum EffectActivationTime
{
	// attack
	AttackReady,
	MoveWhileAttack,
	InstanceAttack,
	AfterDoingAttack,
}

public enum EffectTarget
{
	Caster,
	Target,
	Ground
}

[Serializable]
public class EffectData
{
	/*[SerializeField]public Enum enumData;
	public EffectType type;*/
	public EffectActivationTime effectType;
	public EffectTarget effectTarget;
	public List<AssetReference> effectList;
}

public class EffectPoolingData
{
	public List<ObjectAddressablePoolManager<Transform>> poolManagers = new List<ObjectAddressablePoolManager<Transform>>();
}

public class LevelEffect
{
	public int currentLevel;
	public EffectData data;
	public GameObject effect;
	public int index;

	public LevelEffect(int currentLevel, EffectData data, GameObject effect, int index)
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
	public Vector3 positionMargin;
	public Quaternion rotationMargin;

	public TrackingEffectData(GameObject effect, Transform target, Vector3 marginPos, Quaternion marginRot)
	{
		this.effect = effect;
		this.target = target;
		positionMargin = marginPos;
		rotationMargin = marginRot;
	}
}

public class EffectKey
{
	public readonly EffectData effectData;
	public readonly AssetReference effectReference;
	private GameObject effectObject;
	public GameObject EffectObject
	{
		get { return effectObject; }
		// �ش� ������Ʈ�� �񵿱�� �����ǹǷ�, ���� ���� ������ ���� ���� �ڵ�.
		set { effectObject = isAssigned ? effectObject : value; } 
	}
	public readonly bool isLevelEffect;
	public readonly bool isTrackingEffect;
	public bool isAssigned;
	public readonly ObjectAddressablePoolManager<Transform> poolManager;

	public EffectKey(EffectData data, AssetReference effectRef, bool isLevel, bool isTracking, ObjectAddressablePoolManager<Transform> poolManager)
	{
		effectData = data;
		effectReference = effectRef;
		isLevelEffect = isLevel;
		isTrackingEffect = isTracking;
		isAssigned = false;
		this.poolManager = poolManager;
	}
}

public class LevelEffectKey
{
	public List<EffectKey> levelEffectKeys;
}