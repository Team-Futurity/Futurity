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
	[field: SerializeField] public bool IgnoresAutoTargetMove { get; set; }
	[field: SerializeField] public ColliderType AttackColliderType { get; set; }

	[field: SerializeField] public List<CSAttackAssetSaveData> AttackAssets { get; set;}

	/*// Attack Effect
	[field: SerializeField] public Vector3 EffectOffset { get; set; }
	[field: SerializeField] public Vector3 EffectRotOffset { get; set; }
	[field: SerializeField] public GameObject EffectPrefab { get; set; }
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

	/*// Sound
	[field: SerializeField] public EventReference AttackSound { get; set; }*/

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
		IgnoresAutoTargetMove = node.IgnoresAutoTargetMove;
		AttackColliderType = node.AttackColliderType;

		foreach (var asset in node.AttackAssets)
		{
			CSAttackAssetSaveData data = new CSAttackAssetSaveData();

			data.EffectOffset = asset.AttackEffectAsset.EffectOffset;
			data.EffectRotOffset = asset.AttackEffectAsset.EffectRotOffset;
			data.EffectPrefab = asset.AttackEffectAsset.EffectPrefab;
			data.AttackEffectParent = asset.AttackEffectAsset.AttackEffectParent;

			data.HitEffectOffset = asset.HitEffectAsset.EffectOffset;
			data.HitEffectRotOffset = asset.HitEffectAsset.EffectRotOffset;
			data.HitEffectPrefab = asset.HitEffectAsset.EffectPrefab;
			data.HitEffectParent = asset.HitEffectAsset.AttackEffectParent;

			data.AttackSound = asset.AttackSound;

			AttackAssets.Add(data);
		}

		/*EffectOffset = node.EffectOffset;
		EffectRotOffset = node.EffectRotOffset;
		EffectPrefab = node.EffectPrefab;
		AttackEffectParent = node.AttackEffectParent;

		HitEffectOffset = node.HitEffectOffset;
		HitEffectRotOffset = node.HitEffectRotOffset;
		HitEffectPrefab = node.HitEffectPrefab;
		HitEffectParent = node.HitEffectParent;*/

		AnimInteger = node.AnimInteger;
		ShakePower = node.ShakePower;
		ShakeTime = node.ShakeTime;
		SlowTime = node.SlowTime;
		SlowScale = node.SlowScale;

		/*AttackSound = node.AttackSound;*/

		IsStartingNode = node.IsStartingNode();
	}
}
