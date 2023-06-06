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
			InstantiateEffect(copySpecific, unit.effects[i]);
		}

		InstantiateEffect(copyHit, unit.hitEffect);
		InstantiateEffect(copyHitted, unit.hittedEffect);
	}

	private void InstantiateEffect(List<GameObject> list, Effect effect)
	{
		effect.indexNum = list.Count;
		list.Add(GameObject.Instantiate(effect.effectPrefab, effect.effectTransform == null ? null : effect.effectTransform));
		list[list.Count - 1].SetActive(false);
	}
}