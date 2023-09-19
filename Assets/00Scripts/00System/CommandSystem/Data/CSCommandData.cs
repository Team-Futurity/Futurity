using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CSCommandData
{
	[field: SerializeField] public CSCommandSO NextCommand { get; set; }
}
