using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	private Dictionary<int, BuffBehaviour> activeBuffDic;

	[SerializeField] private List<int> debugBuffList;

    private void Awake()
    {
		activeBuffDic = new Dictionary<int, BuffBehaviour>();
    }

	public void AddBuff(BuffBehaviour buff)
	{
		if(buff is null)
		{
			FDebug.Log("[BuffSystem] �ش� ������ �������� �ʽ��ϴ�.");
			return;
		}

		AddBuffer(buff);
	}

	public void RemoveBuff(int buffCode)
	{
		var hasBuff = HasBuff(buffCode);

		if(!hasBuff)
		{
			FDebug.Log("[BuffSystem] �ش� ������ �������� �ʽ��ϴ�.");
			return;
		}

		activeBuffDic.Remove(buffCode);
		debugBuffList.Remove(buffCode);
	}

	private void AddBuffer(BuffBehaviour buff)
	{
		var uID = buff.BuffData.BuffCode;
		activeBuffDic.Add(uID, buff);
		debugBuffList.Add(uID);
	}

	private bool HasBuff(int buffCode)
	{
		return activeBuffDic.ContainsKey(buffCode);
	}
}