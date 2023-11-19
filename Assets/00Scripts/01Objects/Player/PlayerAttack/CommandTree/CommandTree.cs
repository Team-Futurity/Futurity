using System.Collections.Generic;
using System;
using UnityEngine;
using FMODUnity;

[Serializable]
public class AttackAsset
{
	[Header("타격 이펙트")]
	public Vector3 effectOffset;
	public Quaternion effectRotOffset;
	public GameObject effectPrefab;
	public GameObject effectParent;
	public EffectParent effectParentType;
	[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;

	[Header("피격 이펙트")]
	public Vector3 hitEffectOffset;
	public Quaternion hitEffectRotOffset;
	public GameObject hitEffectPrefab;
	public GameObject hitEffectParent;
	public EffectParent hitEffectParentType;
	[HideInInspector] public ObjectPoolManager<Transform> hitEffectPoolManager;

	[Header("사운드")]
	public EventReference attackSound;

	public void AddPoolManager()
	{
		if (effectPrefab != null)
		{
			effectPoolManager = new ObjectPoolManager<Transform>(effectPrefab, effectParent);
		}

		if (hitEffectPrefab != null)
		{
			hitEffectPoolManager = new ObjectPoolManager<Transform>(hitEffectPrefab, hitEffectParent);
		}
	}
}


[Serializable]
public class AttackNode
{
	[Header("명령어명")]
	public string name;

	[Header("입력 커맨드")]
	public PlayerInputEnum command;
	[Header("상하위 노드")]
	public List<AttackNode> childNodes;
	public AttackNode parent;

	[Header("공격 관련")]
	public float attackLength;
	public float attackAngle;
	public float attackLengthMark;
	public float attackDelay;
	public float attackSpeed;
	public float attackAfterDelay;
	public float attackST;
	public float attackKnockback;
	public bool ignoresAutoTargetMove;
	public ColliderType attackColliderType;

	public Dictionary<int, AttackAsset> attackAssetsByPart; 

	[Header("연출 관련")]
	public int animInteger;

	public float shakePower;
	public float shakeTime;
	public float slowTime;
	public float slowScale;
	public float rumbleLow;
	public float rumbleHigh;
	public float rumbleDuration;

	[Header("사운드")]
	public EventReference attackVoice;

	private int DefaultPartCode = 404;

	public AttackNode(PlayerInputEnum command)
	{
		this.command = command;
		childNodes = new List<AttackNode>();
		attackAssetsByPart = new Dictionary<int, AttackAsset>();
	}

	public AttackAsset GetAttackAsset(int partCode)
	{
		AttackAsset asset;
		if (attackAssetsByPart.TryGetValue(partCode, out asset)) { return asset; }
		if(attackAssetsByPart.TryGetValue(DefaultPartCode, out asset)) { return asset; }

		return null;
	}
}

[Serializable]
public class CommandTree
{
	[field:SerializeField] public AttackNode Top { get; private set; } // �ֻ�� ���

	public CommandTree()
	{
		Top = new AttackNode(PlayerInputEnum.None);
	}

	public void AddNode(AttackNode newNode, AttackNode parent, int partCode)
	{
		if (!IsNodeInTree(parent)) { FDebug.LogError("[CommandTree] This Node is not Node in This Tree"); return; }

		newNode.parent = parent;
		newNode.GetAttackAsset(partCode).AddPoolManager();
		parent.childNodes.Add(newNode);
	}

	#region Checker
	public bool IsTopNode(AttackNode node) => node == Top;

	/*public void SetTree(AttackNode curNode, AttackNode parent)
	{
		int childCount = 0;
		curNode.parent = parent;
		curNode.AddPoolManager();

		while (curNode.childNodes.Count > childCount)
		{
			SetTree(curNode.childNodes[childCount++], curNode);
		}
	}*/

	private bool IsNodeInTree(AttackNode node)
	{
		var parent = node; 
		while(parent.parent != null)
		{
			parent = parent.parent;
		}

		return parent == Top;
	}
	#endregion

	#region Find
	private AttackNode FindProc(PlayerInputEnum targetInput, AttackNode curNode)
	{
		AttackNode returnNode = null;

		for (int i = 0; i < curNode.childNodes.Count; i++)
		{
			if(curNode.childNodes[i].command == targetInput)
			{
				returnNode = curNode.childNodes[i];
				break;
			}
		}

		return returnNode;
	}

	public AttackNode FindNode(PlayerInputEnum targetInput, AttackNode curNode = null)
	{
		AttackNode resultNode = null;
		if (Top != null) // top�� �����ϰ�
		{
			resultNode = FindProc(targetInput, curNode.childNodes == null ? Top : curNode);
		}
		return resultNode;
	}
	#endregion
}
