using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	// ������ �߰��ϴ� List
	[field: SerializeField] public List<BuffBehaviour> BuffList;
	// ������ �����ϴ� Dic
	private Dictionary<BuffName, BuffBehaviour> buffDic;
    
	// Test Code
    public UnitBase test;

    private void Awake()
    {
		buffDic = new Dictionary<BuffName, BuffBehaviour>();

		if(BuffList is not null)
		{
			foreach(var buff in BuffList)
			{
				buffDic.Add(buff.CurrBuffName, buff);
			}
		}
		
	    OnBuff(BuffName.SHOCK, test);
    }
    
    public void OnBuff(BuffName buffName, UnitBase unit)
    {
		var haveBuff = HaveBuff(buffName);

		if (!haveBuff)
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
    
	private bool HaveBuff(BuffName buffName)
	{
		return buffDic.ContainsKey(buffName);
	}
}
