using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǰ �� Hit Effect�� ������ �ִ� �����ͺ��̽�
// ���⼭ Hit�� ��ü�� Enemy
public class HitEffectDatabase : MonoBehaviour
{
	public List<HitEffectByPart> hitEffectByParts = new List<HitEffectByPart>();
	private Dictionary<int, HitEffectByPart> hitEffectByPartDictionary = new Dictionary<int, HitEffectByPart>();

	public void SetHitEffectDatabase()
	{
		foreach(var part in hitEffectByParts)
		{
			if (hitEffectByPartDictionary.ContainsKey(part.partCode)) { continue; }

			hitEffectByPartDictionary.Add(part.partCode, part);	
		}	
	}

	public HitEffectByPart? GetHitEffect(int partCode)
	{
		if (!hitEffectByPartDictionary.ContainsKey(partCode)) { return null; }

		return hitEffectByPartDictionary[partCode];
	}
}

public struct HitEffectByPart
{
	public int partCode;
	public GameObject hitEffectPrefab;
	public ObjectPoolManager<Transform> poolManager;
	public Vector3 hitEffectOffset;
}
