using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static EnemyEffectManager;
using static UnityEngine.GraphicsBuffer;

public class EffectManager
{
	[SerializeField] private EffectDatas effectDatas;
	//[SerializeField] private GameObject effectParent;
	[SerializeField] private GameObject worldEffectParent;
	private Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectData>>> effectDictionary;
	private Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectPoolingData>>> effectPoolingDictionary = new Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectPoolingData>>>();
	private Dictionary<EffectData, LevelEffect> levelEffectDictionary = new Dictionary<EffectData, LevelEffect>();
	private List<TrackingEffectData> trackingEffects = new List<TrackingEffectData>();

	public EffectManager(EffectDatas effectDatas, /*GameObject effectParent,*/ GameObject worldEffectParent)
	{
		this.effectDatas = effectDatas;
		effectDatas.GetDictionary(out effectDictionary);
		effectDatas.GetPoolingDictionary(out effectPoolingDictionary);
		//this.effectParent = effectParent;
		this.worldEffectParent = worldEffectParent;
	}

	// ���� ������ �����͸� ã�ƿ�
	private int FindTrackingData(GameObject effect)
	{
		for (int effectCount = 0; effectCount < trackingEffects.Count; effectCount++)
		{
			if (trackingEffects[effectCount].effect == effect)
			{
				return effectCount;
			}
		}

		return -1;
	}

	// ���� ����
	private void RegisterTracking(GameObject effect, Transform target, Vector3? inputPosition = null, Quaternion? inputRotation = null)
	{
		Vector3 marginPos = inputPosition == null ? Vector3.zero : inputPosition.Value - target.position;
		Quaternion rot = new Quaternion();
		rot.eulerAngles = inputRotation == null ? Vector3.zero : inputRotation.Value.eulerAngles - target.rotation.eulerAngles;

		trackingEffects.Add(new TrackingEffectData(effect, target, marginPos, rot));
	}

	// �˸��� EffectData�� ��ȯ
	private EffectData GetEffectData(int index, EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster)
	{
		Dictionary<EffectTarget, List<EffectData>> tempDictionary;
		List<EffectData> effectDatas;

		if (!effectDictionary.TryGetValue(activationTime, out tempDictionary)) { FDebug.LogWarning("[EffectManager] Failed to Get Effect. Because of <ActivationTime> Mismatch"); return null; }
		if (!tempDictionary.TryGetValue(target, out effectDatas)) { FDebug.LogWarning("[EffectManager] Failed to Get Effect. Because of <Traget> Mismatch"); return null; }

		tempDictionary = null;

		return effectDatas[index];
	}

	private ObjectAddressablePoolManager<Transform> GetObjectPoolManager(int index, EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster, int listIndex = 0)
	{
		Dictionary<EffectTarget, List<EffectPoolingData>> tempDictionary;
		List<EffectPoolingData> effectPools;

		if (!effectPoolingDictionary.TryGetValue(activationTime, out tempDictionary)) { FDebug.LogWarning("[EffectManager] Failed to Get ObjectPoolManager. Because of <ActivationTime> Mismatch"); return null; }
		if (!tempDictionary.TryGetValue(target, out effectPools)) { FDebug.LogWarning("[EffectManager] Failed to Get ObjectPoolManager. Because of <Traget> Mismatch"); return null; }

		tempDictionary = null;

		return effectPools[index].poolManagers[listIndex];
	}

	// �������� Ű���� üũ
	private bool CheckEffectKey(EffectKey key)
	{
		if (key == null) { FDebug.LogWarning("[EffectManager] This key is null"); return false; }

		return true;
	}

	// ����Ʈ ����
	/// <summary>
	/// ����Ʈ�� �����ϴ� �Լ�
	/// </summary>
	/// <param name="activationTime">���� �ñ�</param>
	/// <param name="target">���� ���</param>
	/// <param name="isLevel">�ܰ��� ����Ʈ����</param>
	/// <param name="trackingTarget">��� ���� ���(������ null)</param>
	/// <param name="position">���� ��ġ</param>
	/// <param name="rotation">ȸ�� ��</param>
	/// <param name="localParent">������ ����Ʈ�� �θ�. �⺻������ Local�̸�, null�� World</param>
	/// <param name="index">SO���� ���� ActivationTime, Target�� ����� Effect Index</param>
	/// <param name="effectListIndex">EffectData�� ��� Effect Index</param>
	/// <returns>����Ʈ �����Ͱ� ��� Ű �� (readonly)</returns>
	public EffectKey ActiveEffect(EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster, 
		Vector3? position = null, quaternion? rotation = null, GameObject localParent = null,
		Transform trackingTarget = null, bool isLevel = false, 
		int index = 0, int effectListIndex = 0)
	{
		Vector3 pos = position ?? Vector3.zero;
		quaternion rot = rotation ?? Quaternion.identity;
		localParent = localParent == null ? worldEffectParent : localParent;
		bool isTracking = trackingTarget != null;

		EffectData effectData = GetEffectData(index, activationTime, target);

		// addressable�� �񵿱� ����
		AsyncOperationHandle<GameObject> effectObject = new AsyncOperationHandle<GameObject>();
		var poolManager = GetObjectPoolManager(index, activationTime, target, effectListIndex);
		poolManager.SetManager(effectData.effectList[effectListIndex], localParent);
		var obj = poolManager.ActiveObject(ref effectObject, position, rotation);
		//var effectObject = effectData.effectList[effectListIndex].InstantiateAsync(pos, rot, localParent); //Instantiate(effectData.effectList[effectListIndex], pos, rot, effectParent.transform);
		
		// Ű ����
		EffectKey key = new EffectKey(effectData, effectData.effectList[effectListIndex], isLevel, isTracking, poolManager);
		
		// ������Ʈ ���� �Ϸ� �� ó��
		//effectObject.Completed += (AsyncOperationHandle<GameObject> obj) => 
			{
				// Ű�� ������Ʈ �Ҵ�
				//key.EffectObject = obj.Result;
				key.EffectObject = obj.gameObject;

				// ��������
				if (isTracking)
				{
					//RegisterTracking(obj.Result, trackingTarget, position, rotation);
					RegisterTracking(obj.gameObject, trackingTarget, position, rotation);
				}

				if(isLevel)
				{
					// �ܰ� ����
					//EffectData effectData = GetEffectData(index, activationTime, target);
					LevelEffect levelEffect;

					// ���� �̹� �����ϴ� ����Ʈ���
					if (levelEffectDictionary.TryGetValue(effectData, out levelEffect))
					{
						levelEffect.currentLevel = effectListIndex;
						levelEffect.effect = key.EffectObject;
						levelEffect.data = effectData;
						levelEffect.index = index;
					}
					else
					{
						levelEffect = new LevelEffect(0, effectData, key.EffectObject, index);
						levelEffectDictionary.Add(effectData, levelEffect);
					}
				}
			};

		return key;
	}

	// �ܰ� ��� ����Ʈ �ܰ� ����
	public void SetEffectLevel(ref EffectKey currentKey, int level)
	{
		if (!CheckEffectKey(currentKey)) { return; }
		if (level < 0) { FDebug.LogWarning("[EffectManager] This Level is too low. Please use a value greater than or equal to Zero"); return; }
		if (!currentKey.isLevelEffect) { FDebug.LogWarning("[EffectManager] This key is not Level Effect Key"); return; }

		// ������ Read
		LevelEffect levelEffect;
		if (!levelEffectDictionary.TryGetValue(currentKey.effectData, out levelEffect)) { FDebug.LogWarning("[EffectManager] This key is not valid."); return; }

		// RushLevelEffect ��� �� ����
		EffectData data = levelEffect.data;
		int trackingNumber = FindTrackingData(levelEffect.effect);
		bool isTrace = trackingNumber >= 0;
		Transform traceTarget = isTrace ? trackingEffects[trackingNumber].target : null;

		// �� �ܰ� ����Ʈ ����
		FDebug.Log($"data : {data}, levelEffect : {levelEffect}");
		FDebug.Log($"level : {levelEffect.currentLevel}, data : {levelEffect.data}, effect : {levelEffect.effect}, level : {levelEffect.index}");
		var newKey = ActiveEffect(data.effectType, data.effectTarget, 
			levelEffect.effect.transform.position, levelEffect.effect.transform.rotation, levelEffect.effect.transform.parent.gameObject,
			traceTarget, true, levelEffect.index, level);

		// ����Ʈ ���� �� �ܰ� ����
		RemoveEffect(currentKey, trackingNumber);
		levelEffect.currentLevel = level;
		levelEffect.effect = currentKey.EffectObject;
		currentKey = newKey;
	}

	public void RemoveEffect(EffectKey key, int? trackingNumber = null, bool isUnleveling = false)
	{
		if (key == null) { return; }

		LevelEffect obj;

		// Tracking ����
		int num = trackingNumber ?? FindTrackingData(key.EffectObject);
		if (num >= 0)
		{
			trackingEffects.RemoveAt(num);
		}

		// Leveling ����
		if (levelEffectDictionary.TryGetValue(key.effectData, out obj))
		{
			if(isUnleveling)
			{
				levelEffectDictionary.Remove(key.effectData);
			}
		}

		// Deactive
		key.poolManager.DeactiveObject(key.EffectObject.transform);
	}

	public void LateUpdate()
	{
		if (trackingEffects.Count <= 0) { return; }

		List<TrackingEffectData> list = null;

		// ���� ������ ������Ʈ �̵�
		foreach (var effectData in trackingEffects)
		{
			if (effectData.effect == null)
			{
				if (list == null) { list = new List<TrackingEffectData>(); }

				list.Add(effectData);
				continue;
			}

			effectData.effect.transform.position = effectData.target.position + effectData.positionMargin;
			effectData.effect.transform.eulerAngles = effectData.target.rotation.eulerAngles + effectData.rotationMargin.eulerAngles;
		}

		if (list == null) { return; }

		foreach (var data in list)
		{
			trackingEffects.Remove(data);
		}
	}
}
