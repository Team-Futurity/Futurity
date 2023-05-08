using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	// ������ ���������� ����ϴ� ���� Case
    [field: SerializeField] public List<BuffData> BuffList { get; private set; }

    private Dictionary<BuffName, BuffData> buffDic;

    private void Awake()
    {
	    buffDic = new Dictionary<BuffName, BuffData>();

	    foreach (var buff in BuffList)
	    {
		    buffDic.Add(buff.BuffBehaviour.currBuffName, buff);
	    }
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
	    
	    // Active�� Buff ����
	    buffDic[buffName].Active(unit);
    }
    
}
