using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EffectDatas", menuName = "ScriptableObject/Effect/EffectDatas")]
public class EffectDatas : ScriptableObject
{
	public List<EffectData> effectDatas = new List<EffectData>();

	public void GetDictionary(out Dictionary<(EffectActivationTime, EffectTarget), List<EffectData>> effectDictionary)
	{
		// Dictionary 생성
		effectDictionary = new Dictionary<(EffectActivationTime, EffectTarget), List<EffectData>>();

		// 실제 데이터 삽입
		foreach (var effect in effectDatas)
		{
			List<EffectData> effectList;
			if (!effectDictionary.TryGetValue((effect.effectType, effect.effectTarget), out effectList))
			{ 
				effectList = new List<EffectData>();
				effectDictionary.Add((effect.effectType, effect.effectTarget), effectList); 
			}

			effectList.Add(effect);
		}
	}

	public void GetPoolingDictionary(out Dictionary<(EffectActivationTime, EffectTarget), List<EffectPoolingData>> effectDictionary)
	{
		// Dictionary 생성
		effectDictionary = new Dictionary<(EffectActivationTime, EffectTarget), List<EffectPoolingData>>();

		// 실제 데이터 삽입
		foreach (var effect in effectDatas)
		{
			List<EffectPoolingData> effectPoolList;

			if (!effectDictionary.TryGetValue((effect.effectType, effect.effectTarget), out effectPoolList))
			{ 
				effectPoolList = new List<EffectPoolingData>();
				effectDictionary.Add((effect.effectType, effect.effectTarget), effectPoolList); 
			}

			var poolData = new EffectPoolingData();
			effectPoolList.Add(poolData);
			foreach (var listEffect in effect.effectList)
			{
				poolData.poolManagers.Add(new ObjectAddressablePoolManager<Transform>(listEffect));
			}
		}
	}
}
