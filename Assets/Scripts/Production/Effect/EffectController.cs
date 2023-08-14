using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

public class EffectController
{
	[SerializeField] private EffectDatas effectDatas;
	//[SerializeField] private GameObject effectParent;
	[SerializeField] private GameObject worldEffectParent;
	private Dictionary<(EffectActivationTime, EffectTarget), List<EffectData>> effectDictionary;
	private Dictionary<(EffectActivationTime, EffectTarget), List<EffectPoolingData>> effectPoolingDictionary = new Dictionary<(EffectActivationTime, EffectTarget), List<EffectPoolingData>>();
	private Dictionary<EffectData, LevelEffect> levelEffectDictionary = new Dictionary<EffectData, LevelEffect>();
	private List<TrackingEffectData> trackingEffects = new List<TrackingEffectData>();

	public EffectController(EffectDatas effectDatas, /*GameObject effectParent,*/ GameObject worldEffectParent)
	{
		this.effectDatas = effectDatas;
		effectDatas.GetDictionary(out effectDictionary);
		effectDatas.GetPoolingDictionary(out effectPoolingDictionary);
		//this.effectParent = effectParent;
		this.worldEffectParent = worldEffectParent;
	}

	#region GetDatas
	// �˸��� EffectData�� ��ȯ
	private EffectData GetEffectData(int index, EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster)
	{
		List<EffectData> effectDatas;

		if (!effectDictionary.TryGetValue((activationTime, target), out effectDatas)) { FDebug.LogWarning("[EffectManager] Failed to Get Effect. Because of <ActivationTime, Traget> Mismatch"); return null; }

		return effectDatas[index];
	}

	private ObjectAddressablePoolManager<Transform> GetObjectPoolManager(int index, EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster, int listIndex = 0)
	{
		List<EffectPoolingData> effectPools;

		if (!effectPoolingDictionary.TryGetValue((activationTime, target), out effectPools)) { FDebug.LogWarning("[EffectManager] Failed to Get ObjectPoolManager. Because of <ActivationTime, Traget> Mismatch"); return null; }

		return effectPools[index].poolManagers[listIndex];
	}
	#endregion

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
		Vector3? position = null, Quaternion? rotation = null, GameObject localParent = null,
		int index = 0, int effectListIndex = 0)
	{
		
		localParent = localParent == null ? worldEffectParent : localParent;

		EffectData effectData = GetEffectData(index, activationTime, target);

		// addressable�� �񵿱� ����
		AsyncOperationHandle<GameObject> effectObject = new AsyncOperationHandle<GameObject>();
		var poolManager = GetObjectPoolManager(index, activationTime, target, effectListIndex);
		poolManager.SetManager(effectData.effectList[effectListIndex], localParent);
		var obj = poolManager.ActiveObject(ref effectObject, position, rotation);
		
		// Ű ����
		EffectKey key = new EffectKey(effectObject, effectData, effectData.effectList[effectListIndex], poolManager, position, rotation, index);
		key.EffectObject = obj.gameObject;

		return key;
	}

	#region Tracking
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
	public void RegisterTracking(EffectKey key, Transform target)
	{
		if (!CheckEffectKey(key)) { return; }

		Vector3 pos = key.position ?? Vector3.zero;
		Quaternion rot = key.rotation ?? Quaternion.identity;

		Vector3 marginPos = pos != Vector3.zero? pos - target.position : pos;
		rot.eulerAngles = rot == Quaternion.identity ? Vector3.zero : rot.eulerAngles - target.rotation.eulerAngles;

		trackingEffects.Add(new TrackingEffectData(key.EffectObject, target, marginPos, rot));

		key.SetTrackingEffect(true);
	}
	#endregion

	#region Level
	public void RegistLevelEffect(EffectKey key, int level = 0)
	{
		if (!CheckEffectKey(key)) { return; }
		if (level < 0) { FDebug.LogWarning("[EffectManager] This Level is too low. Please use a value greater than or equal to Zero"); return; }
		if (key.IsLevelEffect()) { FDebug.LogWarning("[EffectManager] This key is \"Aleady\" Level Effect Key"); return; }

		// �ܰ� ����
		LevelEffect levelEffect;

		// ���� �̹� �����ϴ� ����Ʈ���
		if (levelEffectDictionary.TryGetValue(key.effectData, out levelEffect))
		{
			levelEffect.currentLevel = level;
			levelEffect.effect = key.EffectObject;
			levelEffect.data = key.effectData;
		}
		else
		{
			levelEffect = new LevelEffect(0, key.effectData, key.EffectObject, key.index);
			levelEffectDictionary.Add(key.effectData, levelEffect);
		}

		key.SetLevelEffect(true);
	}

	// �ܰ� ��� ����Ʈ �ܰ� ����
	public void SetEffectLevel(ref EffectKey currentKey, int level)
	{
		if (!CheckEffectKey(currentKey)) { return; }
		if (level < 0) { FDebug.LogWarning("[EffectManager] This Level is too low. Please use a value greater than or equal to Zero"); return; }
		if (!currentKey.IsLevelEffect()) { FDebug.LogWarning("[EffectManager] This key is not Level Effect Key"); return; }

		// ������ Read
		LevelEffect levelEffect;
		if (!levelEffectDictionary.TryGetValue(currentKey.effectData, out levelEffect)) { FDebug.LogWarning("[EffectManager] This key is not valid."); return; }

		// LevelEffect ��� �� ����
		EffectData data = levelEffect.data;
		int trackingNumber = FindTrackingData(levelEffect.effect);
		bool isTrace = trackingNumber >= 0;
		Transform traceTarget = isTrace ? trackingEffects[trackingNumber].target : null;

		// �� �ܰ� ����Ʈ ����
		var newKey = ActiveEffect(data.effectType, data.effectTarget, 
			levelEffect.effect.transform.position, levelEffect.effect.transform.rotation, levelEffect.effect.transform.parent.gameObject,
			levelEffect.index, level);

		// ���� ���� �ʱ�ȭ
		if (isTrace) { RegisterTracking(newKey, traceTarget); }

		// ����Ʈ ���� �� �ܰ� ����
		RemoveEffect(currentKey, trackingNumber);
		levelEffect.currentLevel = level;
		levelEffect.effect = currentKey.EffectObject;
		levelEffect.data = data;
		newKey.SetLevelEffect(true);
		currentKey = newKey;
	}
	#endregion

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
