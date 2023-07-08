using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMManager : Singleton<EMManager>
{
	public GameObject worldEffectParent;
	private Dictionary<EffectDatas, EffectManager> emDictionary = new Dictionary<EffectDatas, EffectManager>();
	
	/// <summary>
	/// EffectManager�� ȹ���ϴ� �Լ�
	/// </summary>
	/// <param name="effectSO">Scriptable Object</param>
	/// <param name="isCreate">�������� ������ �����ϴ� �ɼ�</param>
	/// <returns>�ش� SO�� Effect Manager</returns>
    public EffectManager GetEffectManager(EffectDatas effectSO, bool isCreate = true)
	{
		EffectManager em = null;
		// �ش� Ű ���� ���� ���
		if (!emDictionary.ContainsKey(effectSO))
		{
			// �����Ѵٸ�
			if (isCreate)
			{
				// ���� ���� �� Add
				em = new EffectManager(effectSO, worldEffectParent);
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
}
