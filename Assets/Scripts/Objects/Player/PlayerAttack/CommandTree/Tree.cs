using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

[Serializable]
public class AttackNode
{
	[Header("���� Ÿ��")]
	public PlayerController.PlayerInput command;
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

	[Header("���ݿ� �ݶ��̴�")]
	public Collider collider;

	[Header("���� ����Ʈ")]
	public Transform effectPos;
	[SerializeField] private GameObject effectPrefab;
	[SerializeField] private GameObject effectParent;
	[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;

	[Header("�� �ǰ� ����Ʈ")]
	public Vector3 hitEffectOffset;
	[SerializeField] private GameObject hitEffectPrefab;
	[SerializeField] private GameObject hitEffectParent;
	[HideInInspector] public ObjectPoolManager<Transform> hitEffectPoolManager;

	[Header("����� ������")]
	public int animInteger;
	//public float moveDistance = 0f;

	public float randomShakePower;
	public float curveShakePower;
	public float shakeTime;

	[Header("���� ����")]
	public EventReference attackSound;

	public AttackNode(PlayerController.PlayerInput command)
	{
		this.command = command;
		childNodes = new List<AttackNode>();
		effectPoolManager = new ObjectPoolManager<Transform>(effectPrefab, effectParent);
		hitEffectPoolManager = new ObjectPoolManager<Transform>(hitEffectPrefab, hitEffectParent);
		//collider.enabled = false;
	}

/*	public AttackNode(AttackNode node)
	{
		Copy(node);
	}*/

/*	public void Copy(AttackNode node)
	{
		command = node.command;
		childNodes = node.childNodes;
		parent = node.parent;
		attackLength = node.attackLength;
		attackSpeed = node.attackSpeed;
		attackST = node.attackST;
		collider = node.collider;
		effectPrefab = node.effectPrefab;
		effectParent = node.effectParent;
		effectPoolManager = new  ObjectPoolManager<Transform>(effectPrefab, effectParent);
		*//*ObjectPoolManager<Transform> manager = effectParent.AddComponent<ObjectPoolManager<Transform>>();
		manager.SetManager(effectPrefab, effectParent);*//*
	}*/

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
public class Tree
{
	public AttackNode top; // �ֻ�� ���
	[HideInInspector] public int dataCount;

	public Tree()
	{
		top = new AttackNode(PlayerController.PlayerInput.None);
	}

	public void SetTree(AttackNode curNode, AttackNode parent)
	{
		int childCount = 0;
		curNode.parent = parent;
		curNode.AddPoolManager();

		while (curNode.childNodes.Count > childCount)
		{
			SetTree(curNode.childNodes[childCount++], curNode);
		}
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
	private AttackNode FindProc(PlayerController.PlayerInput targetInput, AttackNode curNode)
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

	public AttackNode FindNode(PlayerController.PlayerInput targetInput, AttackNode curNode = null)
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
