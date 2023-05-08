using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	// 버프를 직접적으로 사용하는 시전 Case
    [field: SerializeField] public List<BuffData> BuffList { get; private set; }

    public void OnBuff()
    {
	    // Buff를 실행시키는 Method
    }
    
}
