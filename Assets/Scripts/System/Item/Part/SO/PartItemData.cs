using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartItemData : ScriptableObject
{
	// Part Code
	[field: SerializeField] public int PartCode { get; private set; }

}
