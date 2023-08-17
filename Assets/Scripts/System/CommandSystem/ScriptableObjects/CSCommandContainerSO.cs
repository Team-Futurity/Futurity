using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCommandContainerSO : ScriptableObject
{
	[field: SerializeField] public string FileName { get; set; }
	[field: SerializeField] public SerializableDictionary<CSCommandGroupSO, List<CSCommandSO>> CommandGroups { get; set; }
	[field: SerializeField] public List<CSCommandSO> UngroupedCommands { get; set; }

	public void Initialize(string fileName)
	{
		FileName = fileName;

		CommandGroups = new SerializableDictionary<CSCommandGroupSO, List<CSCommandSO>>();
		UngroupedCommands = new List<CSCommandSO>();
	}
}
