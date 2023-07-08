using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectManager : Singleton<EffectManager>
{
	[SerializeField] private EffectDatas effectDatas;
	//[SerializeField] private GameObject effectParent;
	[SerializeField] private GameObject worldEffectParent;
	private Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectData>>> effectDictionary;
	private Dictionary<EffectKey, LevelEffect> levelEffectDictionary = new Dictionary<EffectKey, LevelEffect>();
	private List<TrackingEffectData> trackingEffects = new List<TrackingEffectData>();

	public EffectManager(EffectDatas effectDatas, /*GameObject effectParent,*/ GameObject worldEffectParent)
	{
		this.effectDatas = effectDatas;
		//this.effectParent = effectParent;
		this.worldEffectParent = worldEffectParent;
	}

	private void Start()
	{
		effectDatas.GetDictionary(out effectDictionary);
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
	/// <param name="index">SO���� ���� ActivationTime, Target�� ����� Effect Index</param>
	/// <param name="effectListIndex">EffectData�� ��� Effect Index</param>
	/// <returns>����Ʈ �����Ͱ� ��� Ű �� (readonly)</returns>
	public EffectKey ActiveEffect(EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster, 
		Vector3? position = null, quaternion? rotation = null,
		bool isLevel = false, Transform trackingTarget = null,
		int index = 0, int effectListIndex = 0)
	{
		Vector3 pos = position ?? Vector3.zero;
		quaternion rot = rotation ?? Quaternion.identity;
		bool isTracking = trackingTarget != null;

		EffectData effectData = GetEffectData(index, activationTime, target);

		// addressable�� �񵿱� ����
		var effectObject = effectData.effectList[effectListIndex].InstantiateAsync(pos, rot, effectParent.transform); //Instantiate(effectData.effectList[effectListIndex], pos, rot, effectParent.transform);
		
		// Ű ����
		EffectKey key = new EffectKey(effectData.effectList[effectListIndex], isLevel, isTracking);
		
		// ������Ʈ ���� �Ϸ� �� ó��
		effectObject.Completed += (AsyncOperationHandle<GameObject> obj) => 
			{
				// Ű�� ������Ʈ �Ҵ�
				key.EffectObject = obj.Result;

				// ��������
				if (isTracking)
				{
					RegisterTracking(obj.Result, trackingTarget, position, rotation);
				}

				if(isLevel)
				{
					// �ܰ� ����
					EffectData effectData = GetEffectData(index, activationTime, target);
					LevelEffect levelEffect;

					// ���� �̹� �����ϴ� ����Ʈ���
					if (levelEffectDictionary.TryGetValue(key, out levelEffect))
					{
						levelEffect.currentLevel = effectListIndex;
						levelEffect.effect = key.EffectObject;
						levelEffect.data = effectData;
						levelEffect.index = index;
					}
					else
					{
						levelEffect = new LevelEffect(0, effectData, key.EffectObject, index);
						levelEffectDictionary.Add(key, levelEffect);
					}
				}
			};

		return key;
	}

	// �ܰ� ��� ����Ʈ �ܰ� ����
	public EffectKey SetEffectLevel(EffectKey currentKey, int level)
	{
		if (!CheckEffectKey(currentKey)) { return null; }
		if (!currentKey.isLevelEffect) { FDebug.LogWarning("[EffectManager] This key is not Level Effect Key"); return null; }

		// ������ Read
		LevelEffect levelEffect;
		if (!levelEffectDictionary.TryGetValue(currentKey, out levelEffect)) { FDebug.LogWarning("[EffectManager] This key is not valid."); return null; }

		// RushLevelEffect ��� �� ����
		EffectData data = levelEffect.data;
		int trackingNumber = FindTrackingData(levelEffect.effect);
		bool isTrace = trackingNumber >= 0;
		Transform traceTarget = isTrace ? trackingEffects[trackingNumber].target : null;
		EffectKey nextEffect;

		// �� �ܰ� ����Ʈ ����
		FDebug.Log($"data : {data}, levelEffect : {levelEffect}");
		FDebug.Log($"level : {levelEffect.currentLevel}, data : {levelEffect.data}, effect : {levelEffect.effect}, level : {levelEffect.index}");
		nextEffect = ActiveEffect(data.effectType, data.effectTarget, levelEffect.effect.transform.position, levelEffect.effect.transform.rotation, true, traceTarget, levelEffect.index, level);

		// ����Ʈ ���� �� �ܰ� ����
		RemoveEffect(levelEffect.effect, trackingNumber);
		levelEffect.currentLevel = level;
		levelEffect.effect = nextEffect.EffectObject;

		return nextEffect;
	}

	// ����Ʈ ����
	private void RemoveEffect(GameObject effect, int? trackingNumber = null)
	{
		int num = trackingNumber ?? FindTrackingData(effect);
		if (num >= 0)
		{
			trackingEffects.RemoveAt(num);
		}

		Destroy(effect);
	}

	public void RemoveEffectByKey(EffectKey key, int? trackingNumber = null)
	{
		LevelEffect obj;
		if (key == null) { return; }
		if (!levelEffectDictionary.TryGetValue(key, out obj)) { FDebug.LogWarning("[EffectManager] This key is not Invalid"); return; }

		
		RemoveEffect(obj.effect, trackingNumber);
	}

	private void LateUpdate()
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
