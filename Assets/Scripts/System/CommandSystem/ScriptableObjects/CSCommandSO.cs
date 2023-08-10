using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CSCommandSO : ScriptableObject
{
	[field: SerializeField] public string CommandName { get; set; }
	[field: SerializeField] public List<CSCommandData> NextCommands { get; set; }
	[field: SerializeField] public CSCommandType CommandType { get; set; }
	[field: SerializeField] public bool IsStartingCommand { get; set; }

	// Combo Data
	[field: SerializeField] public float AttackLength { get; set; }
	[field: SerializeField] public float AttackAngle { get; set; }
	[field: SerializeField] public float AttackLengthMark { get; set; }
	[field: SerializeField] public float AttackDelay { get; set; }
	[field: SerializeField] public float AttackSpeed { get; set; }
	[field: SerializeField] public float AttackAfterDelay { get; set; }
	[field: SerializeField] public float AttackST { get; set; }
	[field: SerializeField] public float AttackKnockback { get; set; }

	public void Initialize(string commandName, List<CSCommandData> nextCommands, CSCommandType type, bool isStartingCommand)
	{
		CommandName = commandName;
		CommandType = type;
		NextCommands = nextCommands;
		IsStartingCommand = isStartingCommand;
	}
}

