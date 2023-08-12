using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

	// Attack Effect
	[field: SerializeField] public Vector3 EffectOffset { get; set; }
	[field: SerializeField] public GameObject EffectPrefab { get; set; }
	[field: SerializeField] public EffectParent AttackEffectParent { get; set; }

	// Enemy Hit Effect
	[field: SerializeField] public Vector3 HitEffectOffset { get; set; }
	[field: SerializeField] public GameObject HitEffectPrefab { get; set; }
	[field: SerializeField] public EffectParent HitEffectParent { get; set; }

	// Production
	[field: SerializeField] public int AnimInteger { get; set; }
	[field: SerializeField] public float RandomShakePower { get; set; }
	[field: SerializeField] public float CurveShakePower { get; set; }
	[field: SerializeField] public float ShakeTime { get; set; }
	[field: SerializeField] public float SlowTime { get; set; }
	[field: SerializeField] public float SlowScale { get; set; }

	// Sound
	[field: SerializeField] public EventReference AttackSound { get; set; }

	// ETC

	[field: SerializeField] public bool IsStartingNode { get; set; }

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

		EffectOffset = node.EffectOffset;
		EffectPrefab = node.EffectPrefab;
		AttackEffectParent = node.AttackEffectParent;

		HitEffectOffset = node.EffectOffset;
		HitEffectPrefab = node.HitEffectPrefab;
		HitEffectParent = node.HitEffectParent;

		AnimInteger = node.AnimInteger;
		RandomShakePower = node.RandomShakePower;
		CurveShakePower = node.CurveShakePower;
		ShakeTime = node.ShakeTime;
		SlowTime = node.SlowTime;
		SlowScale = node.SlowScale;

		AttackSound = node.AttackSound;

		IsStartingNode = node.IsStartingNode();
	}
}
