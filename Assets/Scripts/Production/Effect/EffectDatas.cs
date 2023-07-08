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

			List<EffectData> effectDatas;

			if (!effectDic.TryGetValue(effect.effectTarget, out effectDatas)) 
				{ effectDatas = new List<EffectData>(); effectDic.Add(effect.effectTarget, effectDatas); }

			effectDatas.Add(effect);
		}
	}
}
