using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDatabase : MonoBehaviour
{
	private static Dictionary<int, PartBehaviour> partDataBaseDic;

	private void Awake()
	{
		partDataBaseDic = new Dictionary<int, PartBehaviour>();

		var partArray = gameObject.GetComponentsInChildren<PartBehaviour>();

		for (int i = 0; i < partArray.Length; ++i)
		{
			partDataBaseDic.Add(partArray[i].partCode, partArray[i]);
		}
	}

	public static PartBehaviour GetPart(int code)
	{
		return partDataBaseDic[code];
	}
	
}
