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
	[field: SerializeField] public List<CSNextCommandSaveData> NextComboNodes { get; set; }

	// Combo Data
	[field: SerializeField] public float AttackLength { get; set; }
	[field: SerializeField] public float AttackAngle { get; set; }
	[field: SerializeField] public float AttackLengthMark { get; set; }
	[field: SerializeField] public float AttackDelay { get; set; }
	[field: SerializeField] public float AttackSpeed { get; set; }
	[field: SerializeField] public float AttackAfterDelay { get; set; }
	[field: SerializeField] public float AttackST { get; set; }
	[field: SerializeField] public float AttackKnockback { get; set; }

	public CSNodeSaveData(CSNode node)
	{
		List<CSNextCommandSaveData> commands = node.CloneNodeNextCommands();

		ID = node.ID;
		Name = node.CommandName;
		NextComboNodes = commands;
		GroupID = node.NodeGroup?.ID;
		CommandType = node.CommandType;
		Position = node.GetPosition().position;

		AttackLength = node.AttackLength;
		AttackAngle = node.AttackAngle;
		AttackLengthMark = node.AttackLengthMark;
		AttackDelay = node.AttackDelay;
		AttackSpeed = node.AttackSpeed;
		AttackAfterDelay = node.AttackAfterDelay;
		AttackST = node.AttackST;
		AttackKnockback = node.AttackKnockback;
	}
}
