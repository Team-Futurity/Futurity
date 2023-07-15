using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
	public readonly AsyncOperationHandle<GameObject> handle;
	public readonly EffectData effectData;
	public readonly AssetReference effectReference;
	public GameObject EffectObject
	{
		get { return effectObject; }
		// 해당 오브젝트는 비동기로 생성되므로, 생성 이후 수정을 막기 위한 코드.
		set { effectObject = isAssigned ? effectObject : value; } 
	}
	public bool isAssigned;
	public readonly ObjectAddressablePoolManager<Transform> poolManager;
	public readonly Vector3 position;
	public readonly Quaternion rotation;
	public readonly int index;

	private GameObject effectObject;
	private bool isLevelEffect;
	private bool isTrackingEffect;

	public EffectKey(AsyncOperationHandle<GameObject> handle, EffectData data, AssetReference effectRef, ObjectAddressablePoolManager<Transform> poolManager, Vector3 pos, Quaternion rot, int index)
	{
		effectData = data;
		effectReference = effectRef;
		isLevelEffect = false;
		position = pos;
		rotation = rot;

		this.poolManager = poolManager;
		this.handle = handle;
		this.index = index;

		isTrackingEffect = false;
		isAssigned = false;
	}

	public bool IsLevelEffect() => isLevelEffect;
	public bool IsTrackingEffect() => isTrackingEffect;
	public bool SetLevelEffect(bool isLevel) => isLevelEffect = isLevel;
	public bool SetTrackingEffect(bool isTrack) => isTrackingEffect = isTrack;
}

public class LevelEffectKey
{
	public List<EffectKey> levelEffectKeys;
}