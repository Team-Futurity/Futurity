using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	private Dictionary<int, BuffBehaviour> activeBuffDic;

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
	}

	private void AddBuffer(BuffBehaviour buff)
	{
		var uID = buff.BuffData.BuffCode;
		activeBuffDic.Add(uID, buff);	
	}

	private bool HasBuff(int buffCode)
	{
		return activeBuffDic.ContainsKey(buffCode);
	}
}