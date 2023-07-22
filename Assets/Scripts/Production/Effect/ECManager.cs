using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECManager : Singleton<ECManager>
{
	public GameObject worldEffectParent;
	private Dictionary<EffectDatas, EffectController> emDictionary = new Dictionary<EffectDatas, EffectController>();
	
	/// <summary>
	/// EffectManager�� ȹ���ϴ� �Լ�
	/// </summary>
	/// <param name="effectSO">Scriptable Object</param>
	/// <param name="isCreate">�������� ������ �����ϴ� �ɼ�</param>
	/// <returns>�ش� SO�� Effect Manager</returns>
    public EffectController GetEffectManager(EffectDatas effectSO, bool isCreate = true)
	{
		EffectController em = null;
		// �ش� Ű ���� ���� ���
		if (!emDictionary.ContainsKey(effectSO))
		{
			// �����Ѵٸ�
			if (isCreate)
			{
				// ���� ���� �� Add
				em = new EffectController(effectSO, worldEffectParent);
				emDictionary.Add(effectSO, em);
			}
		}
		else
		{
			// ���� ��� ���� �� �ε�
			em = emDictionary[effectSO];
		}

		// ��ȯ
		return em;
	}

	private void LateUpdate()
	{
		foreach(var data in emDictionary.Values)
		{
			data.LateUpdate();
		}
	}
}
