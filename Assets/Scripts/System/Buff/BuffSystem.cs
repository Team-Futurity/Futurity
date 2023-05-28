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

	// 해당 버프가 있는지 확인한다.
	public bool HasSaveBuff(int buffCode)
	{
		return buffSaveDic.ContainsKey(buffCode);
	}

	public BuffBehaviour GetSaveBuff(int buffCode)
	{
		return buffSaveDic[buffCode];
	}

	// 버프를 적용한다.
	public void SetActiveBuff(BuffBehaviour buff)
    {
		if(buff is null)
		{
			FDebug.Log($"{buff}이(가) 존재하지 않습니다.");
			return;
		}

		// Buff Object 생성
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
