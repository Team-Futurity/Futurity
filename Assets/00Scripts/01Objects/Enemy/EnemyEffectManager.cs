using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEffectManager;

public class EnemyEffectManager : Singleton<EnemyEffectManager>
{
	[HideInInspector] public List<GameObject> copySpecific;
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

	public void Start()
	{
		copySpecific = new List<GameObject>();
		copyHit = new List<GameObject>();
		copyHitted = new List<GameObject>();
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
		list.Add(GameObject.Instantiate(effect.effectPrefab, effect.effectTransform == null ? null : effect.effectTransform));
		list[^1].SetActive(false);
		return list.Count - 1;
	}
}