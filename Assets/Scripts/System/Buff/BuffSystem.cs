using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	// 버프를 직접적으로 사용하는 시전 Case
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
	    // 매개 BuffName이 존재하는지 확인한다.
	    var haveKey = buffDic.ContainsKey(buffName);

	    if (!haveKey)
	    {
		    FDebug.Log($"{buffName}이 존재하지 않습니다;");
	    }

	    // 존재할 경우
	    
	    // Active로 Buff 실행
	    buffDic[buffName].Active(unit);
    }
    
}
