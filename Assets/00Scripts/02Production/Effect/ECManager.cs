using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ECManager : Singleton<ECManager>
{
	public GameObject worldEffectParent;
	private Dictionary<EffectDatas, EffectController> emDictionary = new Dictionary<EffectDatas, EffectController>();

	private void Start()
	{
		SceneManager.sceneUnloaded += ResetEffectManagers;
	}

	/// <summary>
	/// EffectManager를 획득하는 함수
	/// </summary>
	/// <param name="effectSO">Scriptable Object</param>
	/// <param name="isCreate">존재하지 않으면 생성하는 옵션</param>
	/// <returns>해당 SO의 Effect Manager</returns>
	public EffectController GetEffectManager(EffectDatas effectSO, bool isCreate = true)
	{
		EffectController em = null;
		// 해당 키 값이 없을 경우
		if (!emDictionary.ContainsKey(effectSO))
		{
			// 생성한다면
			if (isCreate)
			{
				// 새로 생성 후 Add
				em = new EffectController(effectSO, worldEffectParent);
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

	private void FixedUpdate()
	{
		foreach(var data in emDictionary.Values)
		{
			data.FixedUpdate();
		}
	}

	public void ResetEffectManagers(Scene scene)
	{
		foreach (var data in emDictionary.Values)
		{
			data.ResetPoolManager();
		}
	}
}
