using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEffectManager;

public class EnemyEffectManager : Singleton<EnemyEffectManager>
{
	public List<GameObject> copySpecific = new List<GameObject>();
	[HideInInspector] public List<GameObject> copyHit;
	[HideInInspector] public List<GameObject> copyHitted;

	[Serializable]
	public struct Effect
	{
		[HideInInspector] public int indexNum;
		public GameObject effectPrefab;
		public Transform effectTransform;
		public Vector3 effectExtraTransform;
	}


	#region Active/DeActive
	public void SpecificEffectActive(int index)
	{
		copySpecific[index].SetActive(true);
	}

	public void SpecificEffectDeActive(int index)
	{
		copySpecific[index].SetActive(false);
	}

	public void HitEffectActive(int index)
	{
		copyHit[index].SetActive(true);
	}

	public void HitEffectDeActive(int index)
	{
		copyHit[index].SetActive(false);
	}

	public void HittedEffectActive(int index)
	{
		copyHitted[index].SetActive(true);
	}

	public void HittedEffectDeActive(int index)
	{
		copyHitted[index].SetActive(false);
	}
	#endregion

	public void CopyEffect(EnemyController unit)
	{
		if (unit.effects == null || unit.hitEffect.effectPrefab == null)
			return;

		for (int i = 0; i < unit.effects.Count; i++)
		{
			Effect e = unit.effects[i];
			e.indexNum = InstantiateEffect(copySpecific, unit.effects[i]);
			unit.effects[i] = e;
		}

		unit.hitEffect.indexNum = InstantiateEffect(copyHit, unit.hitEffect);
		unit.hittedEffect.indexNum = InstantiateEffect(copyHitted, unit.hittedEffect);
	}

	private int InstantiateEffect(List<GameObject> list, Effect effect)
	{
		var effectObj = Instantiate(effect.effectPrefab, effect.effectTransform == null ? null : effect.effectTransform);
		effectObj.SetActive(false);

		Debug.Log(effectObj.name);

		copySpecific.Add(effectObj);

		return copySpecific.Count - 1;
	}
}