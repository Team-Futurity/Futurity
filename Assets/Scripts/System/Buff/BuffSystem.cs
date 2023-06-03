using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	[field: SerializeField] public List<BuffBehaviour> BuffList { get; private set; }

	private Dictionary<int, BuffBehaviour> buffSaveDic;
	private Dictionary<int, BuffBehaviour> currActiveBuffDic;

    private void Awake()
    {
		buffSaveDic = new Dictionary<int, BuffBehaviour>();
		currActiveBuffDic = new Dictionary<int, BuffBehaviour>();

		SaveBuff();
    }

	// �ش� ������ �ִ��� Ȯ���Ѵ�.
	public bool HasSaveBuff(int buffCode)
	{
		return buffSaveDic.ContainsKey(buffCode);
	}

	public BuffBehaviour GetSaveBuff(int buffCode)
	{
		return buffSaveDic[buffCode];
	}

	// ������ �����Ѵ�.
	public void SetActiveBuff(BuffBehaviour buff)
    {
		if(buff is null)
		{
			FDebug.Log($"{buff}��(��) �������� �ʽ��ϴ�.");
			return;
		}

		// Buff Object ����
		Instantiate(buff, transform.position, Quaternion.identity, transform);
		currActiveBuffDic.Add(buff.BuffData.BuffCode, buff);
	}

	public void RemoveActiveBuff(int buffCode)
	{
		var hasActiveBuff = GetActiveBuff(buffCode);

		if (hasActiveBuff)
		{
			currActiveBuffDic.Remove(buffCode);
		}
	}

	private bool GetActiveBuff(int buffCode)
	{
		return currActiveBuffDic.ContainsKey(buffCode);
	}

	private void SaveBuff()
	{
		if(BuffList is null)
		{
			return;
		}

		foreach(var buff in BuffList)
		{
			buffSaveDic.Add(buff.BuffData.BuffCode, buff);
		}
	}
}
