using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	// ������ ���������� ����ϴ� ���� Case
    [field: SerializeField] public List<BuffData> BuffList { get; private set; }

    private Dictionary<BuffName, BuffData> buffDic;

    public UnitBase test;

    private void Awake()
    {
	    buffDic = new Dictionary<BuffName, BuffData>();

	    foreach (var buff in BuffList)
	    {
	    }
	    
	    OnBuff(BuffName.SHOCK, test);
    }
    
    public void OnBuff(BuffName buffName, UnitBase unit)
    {
	    // �Ű� BuffName�� �����ϴ��� Ȯ���Ѵ�.
	    var haveKey = buffDic.ContainsKey(buffName);

	    if (!haveKey)
	    {
		    FDebug.Log($"{buffName}�� �������� �ʽ��ϴ�;");
	    }

	    // ������ ���
	    var buff = buffDic[buffName];
	    var buffObj = new GameObject();
    }
    
}
