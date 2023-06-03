using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffProvider : MonoBehaviour
{
	[field: SerializeField] public List<BuffBehaviour> BuffList { get; private set; }

	private Dictionary<int, BuffBehaviour> HoldBuffDic;

	private void Awake()
	{
		HoldBuffDic = new Dictionary<int, BuffBehaviour>();
	}

	private void OnEnable()
	{
		SetBuffer();
	}

	public void SetBuff(UnitBase unit, int buffCode)
	{
		ProceedBuff(unit, buffCode);
	}

	public bool HasBuff(int buffCode)
	{
		return HoldBuffDic.ContainsKey(buffCode);
	}

	private void ProceedBuff(UnitBase unit, int buffCode)
	{
		// Object Pooling은 안정화가 된 이후, 수정할 예정
		var hasBuff = HasBuff(buffCode);

		if (!hasBuff)
		{
			FDebug.Log($"{buffCode}에 해당하는 버프가 존재하지 않습니다.");
		}

		var buffObject = Instantiate(GetBuff(buffCode));

		buffObject.Create(unit);
	}

	private BuffBehaviour GetBuff(int buffCode)
	{
		return HoldBuffDic[buffCode];
	}

	private void SetBuffer()
	{
		if (BuffList.Count <= 0) 
		{
			return;
		}

		foreach(var buff in BuffList)
		{
			var uID = buff.BuffData.BuffCode;

			HoldBuffDic.Add(uID, buff);
		}
	}


}
