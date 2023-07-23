using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CSNodeSaveData
{
	[field: SerializeField] public string ID { get; set; }	
	[field: SerializeField] public string Name { get; set; }
	[field: SerializeField] public string GroupID { get; set; }
	[field: SerializeField] public CSCommandType CommandType { get; set; }
	[field: SerializeField] public Vector2 Position { get; set; }
	[field: SerializeField] public string NextComboNodeID { get; set; }	
}
