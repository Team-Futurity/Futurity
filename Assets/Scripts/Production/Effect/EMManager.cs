using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMManager : Singleton<EMManager>
{
	public GameObject worldEffectParent;
	private Dictionary<EffectDatas, EffectManager> emDictionary = new Dictionary<EffectDatas, EffectManager>();
	
	/// <summary>
	/// EffectManager를 획득하는 함수
	/// </summary>
	/// <param name="effectSO">Scriptable Object</param>
	/// <param name="isCreate">존재하지 않으면 생성하는 옵션</param>
	/// <returns>해당 SO의 Effect Manager</returns>
    public EffectManager GetEffectManager(EffectDatas effectSO, bool isCreate = true)
	{
		EffectManager em = null;
		// 해당 키 값이 없을 경우
		if (!emDictionary.ContainsKey(effectSO))
		{
			// 생성한다면
			if (isCreate)
			{
				// 새로 생성 후 Add
				em = new EffectManager(effectSO, worldEffectParent);
				emDictionary.Add(effectSO, em);
			}
		}
		else
		{
			// 있을 경우 기존 값 로드
			em = emDictionary[effectSO];
		}

		// 반환
		return em;
	}
}
