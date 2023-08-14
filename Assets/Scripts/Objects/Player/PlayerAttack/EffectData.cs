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

		// Dictionary ����
		rushEffectDictionary = new Dictionary<EffectType, Dictionary<EffectTarget, List<RushEffectData>>>();

		// Dictionary Value ����
		foreach(var type in Enum.GetValues(typeof(EffectType)))
		{
			rushEffectDictionary.Add((EffectType)type, new Dictionary<EffectTarget, List<RushEffectData>>());
		}

		// ���� ������ ����
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