using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

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
	// 알맞은 EffectData를 반환
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

	// 정상적인 키인지 체크
	private bool CheckEffectKey(EffectKey key)
	{
		if (key == null) { FDebug.LogWarning("[EffectManager] This key is null"); return false; }

		return true;
	}

	// 이펙트 생성
	/// <summary>
	/// 이펙트를 생성하는 함수
	/// </summary>
	/// <param name="activationTime">생성 시기</param>
	/// <param name="target">생성 대상</param>
	/// <param name="isLevel">단계적 이펙트인지</param>
	/// <param name="trackingTarget">계속 따라갈 대상(없으면 null)</param>
	/// <param name="position">생성 위치</param>
	/// <param name="rotation">회전 값</param>
	/// <param name="localParent">생성될 이펙트의 부모. 기본적으로 Local이며, null은 World</param>
	/// <param name="index">SO에서 같은 ActivationTime, Target일 경우의 Effect Index</param>
	/// <param name="effectListIndex">EffectData에 담긴 Effect Index</param>
	/// <returns>이펙트 데이터가 담긴 키 값 (readonly)</returns>
	public EffectKey ActiveEffect(EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster,
		Vector3? position = null, Quaternion? rotation = null, GameObject localParent = null,
		int index = 0, int effectListIndex = 0, bool isWorld = true)
	{
		
		localParent = localParent == null ? worldEffectParent : localParent;

		EffectData effectData = GetEffectData(index, activationTime, target);

		// addressable로 비동기 생성
		AsyncOperationHandle<GameObject> effectObject = new AsyncOperationHandle<GameObject>();
		var poolManager = GetObjectPoolManager(index, activationTime, target, effectListIndex);
		poolManager.SetManager(effectData.effectList[effectListIndex], localParent);
		var obj = poolManager.ActiveObject(ref effectObject, position, rotation, isWorld);
		
		// 키 생성
		EffectKey key = new EffectKey(effectObject, effectData, effectData.effectList[effectListIndex], poolManager, position, rotation, index);
		key.EffectObject = obj.gameObject;

		return key;
	}

	#region Tracking
	// 추적 설정한 데이터를 찾아옴
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

	// 추적 설정
	public TrackingEffectData RegisterTracking(EffectKey key, Transform target)
	{
		if (!CheckEffectKey(key)) { return null; }

		Vector3 pos = key.position ?? Vector3.zero;
		Quaternion rot = key.rotation ?? Quaternion.identity;

		Vector3 marginPos = pos != Vector3.zero? pos - target.position : pos;
		rot.eulerAngles = rot == Quaternion.identity ? Vector3.zero : rot.eulerAngles - target.rotation.eulerAngles;

		var trackingData = new TrackingEffectData(key.EffectObject, target, marginPos, rot);
		trackingEffects.Add(trackingData);

		key.SetTrackingEffect(true);

		return trackingData;
	}
	#endregion

	#region Level
	public void RegistLevelEffect(EffectKey key, int level = 0)
	{
		if (!CheckEffectKey(key)) { return; }
		if (level < 0) { FDebug.LogWarning("[EffectManager] This Level is too low. Please use a value greater than or equal to Zero"); return; }
		if (key.IsLevelEffect()) { FDebug.LogWarning("[EffectManager] This key is \"Aleady\" Level Effect Key"); return; }

		// 단계 세팅
		LevelEffect levelEffect;

		// 만일 이미 존재하는 이펙트라면
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

	// 단계 기반 이펙트 단계 설정
	public void SetEffectLevel(ref EffectKey currentKey, int level)
	{
		if (!CheckEffectKey(currentKey)) { return; }
		if (level < 0) { FDebug.LogWarning("[EffectManager] This Level is too low. Please use a value greater than or equal to Zero"); return; }
		if (!currentKey.IsLevelEffect()) { FDebug.LogWarning("[EffectManager] This key is not Level Effect Key"); return; }

		// 데이터 Read
		LevelEffect levelEffect;
		if (!levelEffectDictionary.TryGetValue(currentKey.effectData, out levelEffect)) { FDebug.LogWarning("[EffectManager] This key is not valid."); return; }

		// LevelEffect 기반 값 정의
		EffectData data = levelEffect.data;
		int trackingNumber = FindTrackingData(levelEffect.effect);
		bool isTrace = trackingNumber >= 0;
		Transform traceTarget = isTrace ? trackingEffects[trackingNumber].target : null;

		// 새 단계 이펙트 생성
		var newKey = ActiveEffect(data.effectType, data.effectTarget, 
			levelEffect.effect.transform.position, levelEffect.effect.transform.rotation, levelEffect.effect.transform.parent.gameObject,
			levelEffect.index, level);

		// 추적 설정 초기화
		if (isTrace) { var trackingData = RegisterTracking(newKey, traceTarget); RunTracking(trackingData); }

		// 이펙트 제거 및 단계 변경
		RemoveEffect(currentKey, trackingNumber);
		levelEffect.currentLevel = level;
		levelEffect.effect = newKey.EffectObject;
		levelEffect.data = data;
		newKey.SetLevelEffect(true);
		currentKey = newKey;
	}
	#endregion

	public void RemoveEffect(EffectKey key, int? trackingNumber = null, bool isUnleveling = false)
	{
		if (key == null) { return; }

		LevelEffect obj;

		// Tracking 해제
		int num = trackingNumber ?? FindTrackingData(key.EffectObject);
		if (num >= 0)
		{
			trackingEffects.RemoveAt(num);
		}

		// Leveling 해제
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


	private void RunTracking(TrackingEffectData effectData)
	{
		if(effectData == null) { return; }

		effectData.effect.transform.position = effectData.target.position + effectData.positionMargin;
		effectData.effect.transform.eulerAngles = effectData.target.rotation.eulerAngles + effectData.rotationMargin.eulerAngles;
	}
	public void FixedUpdate()
	{
		if (trackingEffects.Count <= 0) { return; }
		List<TrackingEffectData> list = new List<TrackingEffectData>();

		// 추적 설정한 오브젝트 이동
		foreach (var effectData in trackingEffects)
		{
			if (effectData.effect == null)
			{
				list.Add(effectData);
				continue;
			}

			RunTracking(effectData);
		}

		if (list == null) { return; }

		foreach (var data in list)
		{
			trackingEffects.Remove(data);
		}
	}

	public void ResetPoolManager()
	{
		foreach(var datas in effectPoolingDictionary)
		{
			var list = datas.Value;

			foreach(var managers in list)
			{
				var managerList = managers.poolManagers;

				foreach( var manager in managerList)
				{
					manager.Reset();
				}	
			}
		}
	}
}
