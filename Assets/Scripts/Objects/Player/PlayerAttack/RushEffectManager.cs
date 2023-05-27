using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RushEffectManager : Singleton<RushEffectManager>
{
	[SerializeField] private EffectData rushEffectData;
	[SerializeField] private GameObject rushEffectParent;
	private Dictionary<EffectType, Dictionary<EffectTarget, List<RushEffectData>>> rushEffectDictionary;
	private Dictionary<RushEffectData, RushLevelEffect> rushLevelEffectDictionary = new Dictionary<RushEffectData, RushLevelEffect>();
	private List<TrackingEffectData> trackingEffects = new List<TrackingEffectData>();

	private void Start()
	{
		rushEffectData.GetDictionary(out rushEffectDictionary);
	}

	// ���� ������ �����͸� ã�ƿ�
	private int FindTrackingData(GameObject effect)
	{
		for(int effectCount = 0; effectCount < trackingEffects.Count; effectCount++)
		{
			if (trackingEffects[effectCount].effect == effect)
			{
				return effectCount;
			}
		}

		return -1;
	}

	// ���� ����
	private void RegisterTracking(GameObject effect, Transform target)
	{
		trackingEffects.Add(new TrackingEffectData(effect, target));
	}

	// �˸��� RushEffectData�� ��ȯ
	private RushEffectData GetRushEffectData(int index, EffectType type, EffectTarget target = EffectTarget.Caster)
	{
		Dictionary<EffectTarget, List<RushEffectData>> tempDictionary;
		List<RushEffectData> effectDatas;

		if (!rushEffectDictionary.TryGetValue(type, out tempDictionary)) { FDebug.LogWarning("[RushEffectManager] Failed to Get Rush Effect. Because of <Type> Mismatch"); return null; }
		if(!tempDictionary.TryGetValue(target, out effectDatas)) { FDebug.LogWarning("[RushEffectManager] Failed to Get Rush Effect. Because of <Traget> Mismatch"); return null; }

		tempDictionary = null;

		return effectDatas[index];
	}

	// ����Ʈ ����
	public GameObject ActiveEffect(EffectType type, EffectTarget target = EffectTarget.Caster, int index = 0, int effectListIndex = 0, Transform trackingTarget = null, Vector3? position = null, quaternion? rotation = null)
	{
		Vector3 pos = position ?? Vector3.zero;
		quaternion rot = rotation ?? Quaternion.identity;

		RushEffectData effectData = GetRushEffectData(index, type, target);
		var effectObject = Instantiate(effectData.effectList[effectListIndex], pos, rot, rushEffectParent.transform);

		// ��������
		if(trackingTarget != null)
		{
			RegisterTracking(effectObject, trackingTarget);
		}

		return effectObject;
	}

	// �ܰ� ��� ����Ʈ ����
	public RushEffectData ActiveLevelEffect(EffectType type, EffectTarget target = EffectTarget.Caster, int index = 0, Transform trackingTarget = null, Vector3? position = null, quaternion? rotation = null)
	{
		var obj = ActiveEffect(type, target, index, 0, trackingTarget, position, rotation);

		// �ܰ� ����
		RushEffectData effectData = GetRushEffectData(index, type, target);
		RushLevelEffect levelEffect;

		if (rushLevelEffectDictionary.TryGetValue(effectData, out levelEffect))
		{
			levelEffect.effect = obj;
			levelEffect.data = effectData;
			levelEffect.index = index;
		}
		else
		{
		    levelEffect = new RushLevelEffect(0, effectData, obj, index);
			rushLevelEffectDictionary.Add(effectData, levelEffect);
		}		

		return effectData;
	}

	// �ܰ� ��� ����Ʈ �ܰ� ����
	public void SetEffectLevel(RushEffectData key, int level)
	{
		// ������ Read
		RushLevelEffect levelEffect;
		if (!rushLevelEffectDictionary.TryGetValue(key, out levelEffect)) { FDebug.LogWarning("[RushEffectManager] This key is not valid."); return; }

		// RushLevelEffect ��� �� ����
		RushEffectData data = levelEffect.data;
		int trackingNumber = FindTrackingData(levelEffect.effect);
		bool isTrace = trackingNumber >= 0;
		Transform traceTarget = isTrace ? trackingEffects[trackingNumber].target : null;
		GameObject nextEffect;

		// �� �ܰ� ����Ʈ ����
		nextEffect = ActiveEffect(data.effectType, data.effectTarget, levelEffect.index, level, traceTarget, levelEffect.effect.transform.position, levelEffect.effect.transform.rotation);

		// ����Ʈ ���� �� �ܰ� ����
		RemoveEffect(levelEffect.effect, trackingNumber);
		levelEffect.currentLevel = level;
		levelEffect.effect = nextEffect;
	}

	// ����Ʈ ����
	public void RemoveEffect(GameObject effect, int? trackingNumber = null)
	{
		int num = trackingNumber ?? FindTrackingData(effect);
		if (num >= 0)
		{
			trackingEffects.RemoveAt(num);
		}

		Destroy(effect);
	}

	public void RemoveEffectByKey(RushEffectData key, int? trackingNumber = null)
	{
		RushLevelEffect obj;
		if(key == null) { return; }
		if (!rushLevelEffectDictionary.TryGetValue(key, out obj)) { FDebug.LogWarning("[RushEffectManager] This key is not Invalid"); return; }

		RemoveEffect(obj.effect, trackingNumber);
	}

	private void Update()
	{
		if(trackingEffects.Count <= 0) { return; }

		// ���� ������ ������Ʈ �̵�
		foreach(var effectData in trackingEffects)
		{
			effectData.effect.transform.position = effectData.target.position;
			effectData.effect.transform.rotation = effectData.target.rotation;
		}
	}
}
