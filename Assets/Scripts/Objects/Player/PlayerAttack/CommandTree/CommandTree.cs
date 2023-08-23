using System.Collections.Generic;
using System;
using UnityEngine;
using FMODUnity;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

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

	[Header("���� ����Ʈ")]
	public Vector3 effectOffset;
	public Quaternion effectRotOffset;
	public GameObject effectPrefab;
	public GameObject effectParent;
	[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;

	[Header("�� �ǰ� ����Ʈ")]
	public Vector3 hitEffectOffset;
	public Quaternion hitEffectRotOffset;
	public GameObject hitEffectPrefab;
	public GameObject hitEffectParent;
	[HideInInspector] public ObjectPoolManager<Transform> hitEffectPoolManager;

	[Header("����� ������")]
	public int animInteger;
	//public float moveDistance = 0f;

	public float randomShakePower;
	public float curveShakePower;
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
	public AttackNode top; // �ֻ�� ���

	public CommandTree()
	{
		top = new AttackNode(PlayerInputEnum.None);
	}

	public void AddNode(AttackNode newNode, AttackNode parent)
	{
		if (!IsNodeInTree(parent)) { FDebug.LogError("[CommandTree] This Node is not Node in This Tree"); return; }

		newNode.parent = parent;
		newNode.AddPoolManager();
		parent.childNodes.Add(newNode);
	}

	public bool IsTopNode(AttackNode node) => node == top;

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

		return parent == top;
	}

/*	#region Insert
	public void InsertNewNode(AttackNode newNode, AttackNode curNode = null, int nodePos = 0)
	{
		if (top != null) // top�� null�� �ƴ� ���
		{
			InsertNewNodeProc(newNode, curNode, nodePos);
		}
		else // top�� �������� ���� ���
		{
			FDebug.LogError("[Tree Error] Top Node is Null");
		}
	}

	private void InsertNewNodeProc(AttackNode newNode, AttackNode curNode = null, int nodePos = 0)
	{
		AttackNode targetNode = top;
		if (curNode != null)
			targetNode = curNode;

		targetNode.childNodes.Insert(nodePos, new AttackNode(newNode));
		newNode.parent = targetNode;
	}
	#endregion*/

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
		if (top != null) // top�� �����ϰ�
		{
			resultNode = FindProc(targetInput, curNode.childNodes == null ? top : curNode);
		}
		return resultNode;
	}
	#endregion
}
