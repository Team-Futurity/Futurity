using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EffectDatas", menuName = "ScriptableObject/Effect/EffectDatas")]
public class EffectDatas : ScriptableObject
{
	public List<EffectData> effectDatas = new List<EffectData>();

	public void GetDictionary(out Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectData>>> effectDictionary)
	{
		effectDictionary = null;

		// Dictionary 积己
		effectDictionary = new Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectData>>>();

		// Dictionary Value 积己
		foreach (var type in Enum.GetValues(typeof(EffectActivationTime)))
		{ 
			effectDictionary.Add((EffectActivationTime)type, new Dictionary<EffectTarget, List<EffectData>>());
		}

		// 角力 单捞磐 火涝
		Dictionary<EffectTarget, List<EffectData>> effectDic = null;
		foreach (var effect in effectDatas)
		{
			if (!effectDictionary.TryGetValue(effect.effectType, out effectDic)) 
				{ FDebug.LogError("[EffectDatas] EffectDictionary is null. Please check to declaration"); return; }

			List<EffectData> effectList;

			if (!effectDic.TryGetValue(effect.effectTarget, out effectList)) 
				{ effectList = new List<EffectData>(); effectDic.Add(effect.effectTarget, effectList); }

			effectList.Add(effect);
		}
	}

	public void GetPoolingDictionary(out Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectPoolingData>>> effectDictionary)
	{
		effectDictionary = null;

		// Dictionary 积己
		effectDictionary = new Dictionary<EffectActivationTime, Dictionary<EffectTarget, List<EffectPoolingData>>>();

		// Dictionary Value 积己
		foreach (var type in Enum.GetValues(typeof(EffectActivationTime)))
		{
			effectDictionary.Add((EffectActivationTime)type, new Dictionary<EffectTarget, List<EffectPoolingData>>());
		}

		// 角力 单捞磐 火涝
		Dictionary<EffectTarget, List<EffectPoolingData>> effectDic = null;
		foreach (var effect in effectDatas)
		{
			if (!effectDictionary.TryGetValue(effect.effectType, out effectDic))
			{ FDebug.LogError("[EffectDatas] EffectDictionary is null. Please check to declaration"); return; }

			List<EffectPoolingData> effectPoolList;

			if (!effectDic.TryGetValue(effect.effectTarget, out effectPoolList))
			{ effectPoolList = new List<EffectPoolingData>(); effectDic.Add(effect.effectTarget, effectPoolList); }

			var poolData = new EffectPoolingData();
			effectPoolList.Add(poolData);
			foreach(var listEffect in effect.effectList)
			{
				poolData.poolManagers.Add(new ObjectAddressablePoolManager<Transform>(listEffect));
			}
		}
	}
}
