/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EffectData", menuName = "ScriptableObject/Effect/EffectData")]
public class EffectData : ScriptableObject
{
    public List<RushEffectData> rushEffectData = new List<RushEffectData>();

	public void GetDictionary(out Dictionary<EffectType, Dictionary<EffectTarget, List<RushEffectData>>> rushEffectDictionary)
	{
		rushEffectDictionary = null;

		// Dictionary 积己
		rushEffectDictionary = new Dictionary<EffectType, Dictionary<EffectTarget, List<RushEffectData>>>();

		// Dictionary Value 积己
		foreach(var type in Enum.GetValues(typeof(EffectType)))
		{
			rushEffectDictionary.Add((EffectType)type, new Dictionary<EffectTarget, List<RushEffectData>>());
		}

		// 角力 单捞磐 火涝
		Dictionary<EffectTarget, List<RushEffectData>> effectDic = null;
		foreach (var effect in rushEffectData)
		{
			if(!rushEffectDictionary.TryGetValue(effect.effectType, out effectDic)) { FDebug.LogError("[EffectData] EffectDictionary is null. Please check to declaration"); return; }

			List<RushEffectData> effectDatas;
			
			if(!effectDic.TryGetValue(effect.effectTarget, out effectDatas)) { effectDatas = new List<RushEffectData>(); effectDic.Add(effect.effectTarget, effectDatas); }

			effectDatas.Add(effect);
		}
	}
}
*/