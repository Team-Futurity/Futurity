using System.Collections.Generic;
using System;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.AddressableAssets;

[Serializable]
public class AttackNode
{
	[Header("��� �̸�")]
	public string name;

	[Header("���� Ÿ��")]
	public PlayerInputEnum command;
	[Header("���� ����(�޺�)")]
	public List<AttackNode> childNodes;
	public AttackNode parent;

	[Header("�޺� ������")]
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

	[Header("���� ����Ʈ")]
	public Vector3 effectOffset;
	public Quaternion effectRotOffset;
	public GameObject effectPrefab;
	public GameObject effectParent;
	public EffectParent effectParentType;
	[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;

	[Header("�� �ǰ� ����Ʈ")]
	public Vector3 hitEffectOffset;
	public Quaternion hitEffectRotOffset;
	public GameObject hitEffectPrefab;
	public GameObject hitEffectParent;
	public EffectParent hitEffectParentType;
	[HideInInspector] public ObjectPoolManager<Transform> hitEffectPoolManager;

	[Header("����� ������")]
	public int animInteger;

	public float shakePower;
	public float shakeTime;
	public float slowTime;
	public float slowScale;

	[Header("���� ����")]
	public EventReference attackSound;

	public AttackNode(PlayerInputEnum command)
	{
		this.command = command;
		childNodes = new List<AttackNode>();
	}

	public void AddPoolManager()
	{
		if(effectPrefab != null)
		{
			effectPoolManager = new ObjectPoolManager<Transform>(effectPrefab, effectParent);
		}
		
		if(hitEffectPrefab != null)
		{
			hitEffectPoolManager = new ObjectPoolManager<Transform>(hitEffectPrefab, hitEffectParent);
		}
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

	public void AddNode(AttackNode newNode, AttackNode parent)
	{
		if (!IsNodeInTree(parent)) { FDebug.LogError("[CommandTree] This Node is not Node in This Tree"); return; }

		newNode.parent = parent;
		newNode.AddPoolManager();
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
