using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSCommandSO : ScriptableObject
{
	[field: SerializeField] public string CommandName { get; set; }
	[field: SerializeField] public CSCommandType CommandType { get; set; }
	[field: SerializeField] public bool IsStartingCommand { get; set; }

	public void Initialize(string commandName, CSCommandType type, bool isStartingCommand)
	{
		CommandName = commandName;
		CommandType = type;
		IsStartingCommand = isStartingCommand;
	}
}

