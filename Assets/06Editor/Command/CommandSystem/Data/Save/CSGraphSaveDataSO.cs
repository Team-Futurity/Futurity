using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSGraphSaveDataSO : ScriptableObject
{
	[field: SerializeField] public string FileName { get; set; }
	[field: SerializeField] public List<CSGroupSaveData> Groups { get; set; }
	[field: SerializeField] public List<CSNodeSaveData> Nodes { get; set; }
	[field: SerializeField] public List<string> OldGroupNames { get; set; }
	[field: SerializeField] public List<string> OldUngroupedNames { get; set; }
	[field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

	public void Initialize(string fileName)
	{
		FileName = fileName;

		Groups = new List<CSGroupSaveData>();
		Nodes = new List<CSNodeSaveData>();
	}
}
