using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
	private Dictionary<int, BuffBehaviour> activeBuffDic;

    private void Awake()
    {
		activeBuffDic = new Dictionary<int, BuffBehaviour>();
    }

}
