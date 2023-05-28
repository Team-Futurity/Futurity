using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	[field: SerializeField] public List<BuffBehaviour> BuffList { get; private set; }

	private Dictionary<int, BuffBehaviour> buffSaveDic;
	private Dictionary<int, BuffBehaviour> currActiveBuff;

    private void Awake()
    {
		buffSaveDic = new Dictionary<int, BuffBehaviour>();
		currActiveBuff = new Dictionary<int, BuffBehaviour>();

		if(BuffList is not null)
		{
			foreach(var buff in BuffList)
			{
				//buffSaveDic.Add(buff.BuffData.BuffName, buff);
			}
		}
    }

	private bool HasBuff(int buffCode)
	{
		return buffSaveDic.ContainsKey(buffCode);
	}

	public void OnBuff(int buffCode, BuffBehaviour buff)
    {
		var hasBuff = HasBuff(buffCode);

		if (!hasBuff)
		{
			FDebug.Log($"{buffCode}이(가) 존재하지 않습니다;");
			return;
		}
	}
}
