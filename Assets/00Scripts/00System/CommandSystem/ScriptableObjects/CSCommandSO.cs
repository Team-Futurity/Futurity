using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CSCommandSO : ScriptableObject
{
	[field: SerializeField] public string CommandName { get; set; }
	[field: SerializeField] public List<CSCommandAssetData> AttackAssets { get; set; }
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
	[field: SerializeField] public bool IgnoresAutoTargetMove { get; set; }
	[field: SerializeField] public ColliderType AttackColliderType { get; set; }

	/*// Attack Effect
	[field: SerializeField] public Vector3 EffectOffset { get; set; }
	[field: SerializeField] public Vector3 EffectRotOffset { get; set; }
	[field: SerializeField] public GameObject EffectPrefab { get; set; }
	[field: SerializeField] public GameObject EffectParent { get; set; }
	[field: SerializeField] public EffectParent AttackEffectParent { get; set; }

	// Enemy Hit Effect
	[field: SerializeField] public Vector3 HitEffectOffset { get; set; }
	[field: SerializeField] public Vector3 HitEffectRotOffset { get; set; }
	[field: SerializeField] public GameObject HitEffectPrefab { get; set; }
	[field: SerializeField] public EffectParent HitEffectParent { get; set; }*/

	// Production
	[field: SerializeField] public int AnimInteger { get; set; }
	[field: SerializeField] public float ShakePower { get; set; }
	[field: SerializeField] public float ShakeTime { get; set; }
	[field: SerializeField] public float SlowTime { get; set; }
	[field: SerializeField] public float SlowScale { get; set; }

	// Attack Sound
	[field: SerializeField] public EventReference AttackVoice { get; set; }

	public void Initialize(string commandName, List<CSCommandAssetData> attackAssets, List<CSCommandData> nextCommands, CSCommandType type, bool isStartingCommand)
	{
		CommandName = commandName;
		AttackAssets = attackAssets;
		CommandType = type;
		NextCommands = nextCommands;
		IsStartingCommand = isStartingCommand;
	}
}

