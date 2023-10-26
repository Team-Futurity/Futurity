using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StateData : ScriptableObject
{
	[field: SerializeField] public int enumNumber { get; set; }
	public virtual void SetDataToState() { }
}