using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ǰ �� Hit Effect�� ������ �ִ� �����ͺ��̽�
// ���⼭ Hit�� ��ü�� Enemy
public class HitEffectDatabase : MonoBehaviour
{
	public List<HitEffectByPart> hitEffectByParts = new List<HitEffectByPart>();
	private Dictionary<int, HitEffectByPart> hitEffectByPartDictionary = new Dictionary<int, HitEffectByPart>();

#if UNITY_EDITOR
	[SerializeField] private bool triggerUpdateDatabase;
#endif

	public void SetHitEffectDatabase()
	{
		foreach(var part in hitEffectByParts)
		{
			if (hitEffectByPartDictionary.ContainsKey(part.partCode)) { continue; }

			var hitEffect = part;
			hitEffect.poolManager = new ObjectPoolManager<Transform>(part.hitEffectPrefab);
			hitEffectByPartDictionary.Add(part.partCode, hitEffect);	
		}
	}

	public HitEffectByPart? GetHitEffect(int partCode)
	{
		if (!hitEffectByPartDictionary.ContainsKey(partCode)) { return null; }

		return hitEffectByPartDictionary[partCode];
	}

#if UNITY_EDITOR
	private void Update()
	{
		if(triggerUpdateDatabase)
		{
			SetHitEffectDatabase();
			triggerUpdateDatabase = false;
		}
	}
#endif
}


[System.Serializable]
public struct HitEffectByPart
{
	public int partCode;
	public GameObject hitEffectPrefab;
	public ObjectPoolManager<Transform> poolManager;
	public Vector3 hitEffectOffset;
}
