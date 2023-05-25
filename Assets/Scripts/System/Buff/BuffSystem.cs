using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	// ������ �߰��ϴ� List
	[field: SerializeField] public List<BuffBehaviour> BuffList { get; private set; }
	// ������ �����ϴ� Dic
	private Dictionary<BuffNameList, BuffBehaviour> buffDic;
    
    private void Awake()
    {
		buffDic = new Dictionary<BuffNameList, BuffBehaviour>();

		if(BuffList is not null)
		{
			foreach(var buff in BuffList)
			{
				buffDic.Add(buff.BuffData.BuffName, buff);
			}
		}
    }

	private bool HasBuff(BuffNameList buffName)
	{
		return buffDic.ContainsKey(buffName);
	}

	public void OnBuff(BuffNameList buffName, UnitBase unit)
    {
		var hasBuff = HasBuff(buffName);

		if (!hasBuff)
		{
			FDebug.Log($"{buffName}��(��) �������� �ʽ��ϴ�;");
			return;
		}

		var buff = buffDic[buffName];

		var buffObj = Instantiate(buff);
		buffObj.gameObject.SetActive(false);

		var unitPos = unit.transform.position;
		buffObj.transform.position = unitPos;
		buffObj.GetComponent<BuffBehaviour>().Active(unit);

		buffObj.gameObject.SetActive(true);
	}
}
