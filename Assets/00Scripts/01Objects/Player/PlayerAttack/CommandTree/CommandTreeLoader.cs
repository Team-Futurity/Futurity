using FMOD;
using System.Collections.Generic;
using UnityEngine;

public class CommandTreeLoader : MonoBehaviour
{
	[SerializeField] private CSCommandContainerSO containerSO;
	[SerializeField] private GameObject localParent;
	[SerializeField] private GameObject worldParent;

	private List<CSCommandSO> GetCommandsAll()
	{
		if (containerSO == null) { FDebug.LogError("[CommandTreeLoader] containerSO is Null"); return null; }

		List<CSCommandSO> commands = new List<CSCommandSO>();

		foreach(var groupedNodes in containerSO.CommandGroups.values)
		{
			commands.AddRange(groupedNodes);
		}

		commands.AddRange(containerSO.UngroupedCommands);

		return commands;
	}

    public CommandTree GetCommandTree()
	{
		var commands = GetCommandsAll();

		if (commands == null) { return null; }

		CommandTree tree = new CommandTree();

		foreach (var node in commands)
		{
			if(node.IsStartingCommand)
			{
				tree.AddNode(CSCommandSOToAttackNode(node), tree.Top);
			}
		}

		return tree;
	}

	public AttackNode CSCommandSOToAttackNode(CSCommandSO commandSO)
	{
		var node = new AttackNode(CommandEnumConverter.CommandTypeToPlayerInput(commandSO.CommandType));

		node.name							= commandSO.CommandName;

		node.attackLength					= commandSO.AttackLength;
		node.attackAngle					= commandSO.AttackAngle;
		node.attackST						= commandSO.AttackST;
		node.attackLengthMark				= commandSO.AttackLengthMark;
		node.attackDelay					= commandSO.AttackDelay;
		node.attackSpeed					= commandSO.AttackSpeed;
		node.attackAfterDelay				= commandSO.AttackAfterDelay;
		node.attackKnockback				= commandSO.AttackKnockback;
		node.ignoresAutoTargetMove			= commandSO.IgnoresAutoTargetMove;
		node.attackColliderType				= commandSO.AttackColliderType;

		node.attackAssetsByPart				= ListToDictionary(commandSO.AttackAssets);
		/*node.effectOffset					= commandSO.EffectOffset;
		node.effectRotOffset				= Quaternion.Euler(commandSO.EffectRotOffset);
		node.effectPrefab					= commandSO.EffectPrefab;
		node.effectParent					= GetEffectParent(commandSO.AttackEffectParent);
		node.effectParentType				= commandSO.AttackEffectParent;

		FDebug.Log($"Name : {node.name}, Effect RotationOffset : {node.effectRotOffset.eulerAngles}");

		node.hitEffectOffset				= commandSO.HitEffectOffset;
		node.hitEffectRotOffset				= new Quaternion(); 
		node.hitEffectRotOffset.eulerAngles = commandSO.HitEffectRotOffset;
		node.hitEffectPrefab				= commandSO.HitEffectPrefab;
		node.hitEffectParent				= GetEffectParent(commandSO.HitEffectParent);
		node.hitEffectParentType			= commandSO.HitEffectParent;*/

		node.animInteger					= commandSO.AnimInteger;
		node.shakePower						= commandSO.ShakePower;
		node.shakeTime						= commandSO.ShakeTime;
		node.slowTime						= commandSO.SlowTime;
		node.slowScale						= commandSO.SlowScale;

		/*node.attackSound					= commandSO.AttackSound;*/

		node.childNodes = new List<AttackNode>();
		foreach(var nextSO in commandSO.NextCommands)
		{
			node.childNodes.Add(CSCommandSOToAttackNode(nextSO.NextCommand));
		}

		return node;
	}

	private GameObject GetEffectParent(EffectParent effectParent)
	{
		switch(effectParent)
		{
			case EffectParent.Local:
				return localParent;
			case EffectParent.World:
				return worldParent;
			default:
				FDebug.LogError("[CommandTreeLoader] Effect Parent is None or Invalid. Please Set Valid Effect Parent");
				return null;
		}
	}

	private Dictionary<int, AttackAsset>ListToDictionary(List<CSCommandAssetData> list)
	{
		Dictionary<int, AttackAsset> dictionary = new Dictionary<int, AttackAsset>();
		foreach (var data in list)
		{
			if (dictionary.ContainsKey(data.PartCode)) { continue; }

			dictionary.Add(data.PartCode, CommandAssetDataToAttackAsset(data));
		}

		return dictionary;
	}

	private AttackAsset CommandAssetDataToAttackAsset(CSCommandAssetData data)
	{
		AttackAsset asset = new AttackAsset();

		asset.effectOffset = data.EffectOffset;
		asset.effectRotOffset = Quaternion.Euler(data.EffectRotOffset);
		asset.effectPrefab = data.EffectPrefab;
		asset.effectParent = GetEffectParent(data.AttackEffectParent);
		asset.effectParentType = data.AttackEffectParent;

		asset.hitEffectOffset = data.HitEffectOffset;
		asset.hitEffectRotOffset = Quaternion.Euler(data.HitEffectRotOffset);
		asset.hitEffectPrefab = data.HitEffectPrefab;
		asset.hitEffectParent = GetEffectParent(data.HitEffectParent);
		asset.hitEffectParentType = data.HitEffectParent;

		asset.attackSound = data.AttackSound;

		asset.AddPoolManager();

		return asset;
	}
}
