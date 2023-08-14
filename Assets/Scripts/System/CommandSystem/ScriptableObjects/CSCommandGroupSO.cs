using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCommandGroupSO : ScriptableObject
{
	[field: SerializeField] public string GroupName { get; set; }

	public void Initialize(string groupName)
	{
		GroupName = groupName;
	}
}
