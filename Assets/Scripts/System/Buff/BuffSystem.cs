using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	[field: SerializeField] public List<BuffBehaviour> BuffList { get; private set; }

	private Dictionary<int, BuffBehaviour> buffSaveDic;


    private void Awake()
    {
		buffSaveDic = new Dictionary<int, BuffBehaviour>();


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

	public void OnBuff(int buffCode, UnitBase unit)
    {
		var hasBuff = HasBuff(buffCode);

		if (!hasBuff)
		{
			FDebug.Log($"{buffCode}이(가) 존재하지 않습니다;");
			return;
		}

		//var buff = buffDic[buffName];

		//var buffObj = Instantiate(buff);
		buffObj.gameObject.SetActive(false);

		var unitPos = unit.transform.position;
		buffObj.transform.position = unitPos;
		buffObj.GetComponent<BuffBehaviour>().Active(unit);

		buffObj.gameObject.SetActive(true);
	}
}
