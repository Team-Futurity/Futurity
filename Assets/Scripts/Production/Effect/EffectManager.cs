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
	private void RegisterTracking(GameObject effect, Transform target, Vector3? inputPosition = null, Quaternion? inputRotation = null)
	{
		Vector3 marginPos = inputPosition == null ? Vector3.zero : inputPosition.Value - target.position;
		Quaternion rot = new Quaternion();
		rot.eulerAngles = inputRotation == null ? Vector3.zero : inputRotation.Value.eulerAngles - target.rotation.eulerAngles;

		trackingEffects.Add(new TrackingEffectData(effect, target, marginPos, rot));
	}

	// 알맞은 EffectData를 반환
	private EffectData GetEffectData(int index, EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster)
	{
		Dictionary<EffectTarget, List<EffectData>> tempDictionary;
		List<EffectData> effectDatas;

		if (!effectDictionary.TryGetValue(activationTime, out tempDictionary)) { FDebug.LogWarning("[EffectManager] Failed to Get Effect. Because of <ActivationTime> Mismatch"); return null; }
		if (!tempDictionary.TryGetValue(target, out effectDatas)) { FDebug.LogWarning("[EffectManager] Failed to Get Effect. Because of <Traget> Mismatch"); return null; }

		tempDictionary = null;

		return effectDatas[index];
	}

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
	/// <param name="index">SO에서 같은 ActivationTime, Target일 경우의 Effect Index</param>
	/// <param name="effectListIndex">EffectData에 담긴 Effect Index</param>
	/// <returns>이펙트 데이터가 담긴 키 값 (readonly)</returns>
	public EffectKey ActiveEffect(EffectActivationTime activationTime, EffectTarget target = EffectTarget.Caster, 
		Vector3? position = null, quaternion? rotation = null,
		bool isLevel = false, Transform trackingTarget = null,
		int index = 0, int effectListIndex = 0)
	{
		Vector3 pos = position ?? Vector3.zero;
		quaternion rot = rotation ?? Quaternion.identity;
		bool isTracking = trackingTarget != null;

		EffectData effectData = GetEffectData(index, activationTime, target);

		// addressable로 비동기 생성
		var effectObject = effectData.effectList[effectListIndex].InstantiateAsync(pos, rot, effectParent.transform); //Instantiate(effectData.effectList[effectListIndex], pos, rot, effectParent.transform);
		
		// 키 생성
		EffectKey key = new EffectKey(effectData.effectList[effectListIndex], isLevel, isTracking);
		
		// 오브젝트 생성 완료 시 처리
		effectObject.Completed += (AsyncOperationHandle<GameObject> obj) => 
			{
				// 키에 오브젝트 할당
				key.EffectObject = obj.Result;

				// 추적설정
				if (isTracking)
				{
					RegisterTracking(obj.Result, trackingTarget, position, rotation);
				}

				if(isLevel)
				{
					// 단계 세팅
					EffectData effectData = GetEffectData(index, activationTime, target);
					LevelEffect levelEffect;

					// 만일 이미 존재하는 이펙트라면
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

	// 단계 기반 이펙트 단계 설정
	public EffectKey SetEffectLevel(EffectKey currentKey, int level)
	{
		if (!CheckEffectKey(currentKey)) { return null; }
		if (!currentKey.isLevelEffect) { FDebug.LogWarning("[EffectManager] This key is not Level Effect Key"); return null; }

		// 데이터 Read
		LevelEffect levelEffect;
		if (!levelEffectDictionary.TryGetValue(currentKey, out levelEffect)) { FDebug.LogWarning("[EffectManager] This key is not valid."); return null; }

		// RushLevelEffect 기반 값 정의
		EffectData data = levelEffect.data;
		int trackingNumber = FindTrackingData(levelEffect.effect);
		bool isTrace = trackingNumber >= 0;
		Transform traceTarget = isTrace ? trackingEffects[trackingNumber].target : null;
		EffectKey nextEffect;

		// 새 단계 이펙트 생성
		FDebug.Log($"data : {data}, levelEffect : {levelEffect}");
		FDebug.Log($"level : {levelEffect.currentLevel}, data : {levelEffect.data}, effect : {levelEffect.effect}, level : {levelEffect.index}");
		nextEffect = ActiveEffect(data.effectType, data.effectTarget, levelEffect.effect.transform.position, levelEffect.effect.transform.rotation, true, traceTarget, levelEffect.index, level);

		// 이펙트 제거 및 단계 변경
		RemoveEffect(levelEffect.effect, trackingNumber);
		levelEffect.currentLevel = level;
		levelEffect.effect = nextEffect.EffectObject;

		return nextEffect;
	}

	// 이펙트 제거
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

		// 추적 설정한 오브젝트 이동
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
