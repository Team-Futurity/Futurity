using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

[Serializable]
public class AttackNode
{
	public PlayerController.PlayerInput command;
	public List<AttackNode> childNodes;
	public AttackNode parent;

	public float attackLength;
	public float attackAngle;
	public float attackLengthMark;
	public float attackDelay;
	public float attackSpeed;
	public float attackAfterDelay;
	public float attackST;

	public int loopCount;
	public float loopDelay;

	public Collider collider;

	public Transform effectPos;
	[SerializeField] private GameObject effectPrefab;
	[SerializeField] private GameObject effectParent;
	[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;

	public float animFloat;
	//public float moveDistance = 0f;

	public float randomShakePower;
	public float curveShakePower;
	public float shakeTime;

	public EventReference attackSound;
	public EventReference attackSound2;

	public AttackNode(PlayerController.PlayerInput command)
	{
		this.command = command;
		childNodes = new List<AttackNode>();
		effectPoolManager = new ObjectPoolManager<Transform>(effectPrefab, effectParent);
		//collider.enabled = false;
	}

	public AttackNode(AttackNode node)
	{
		Copy(node);
	}

	public void Copy(AttackNode node)
	{
		command = node.command;
		childNodes = node.childNodes;
		parent = node.parent;
		attackLength = node.attackLength;
		attackSpeed = node.attackSpeed;
		loopCount = node.loopCount;
		loopDelay = node.loopDelay;
		attackST = node.attackST;
		collider = node.collider;
		effectPrefab = node.effectPrefab;
		effectParent = node.effectParent;
		effectPoolManager = new  ObjectPoolManager<Transform>(effectPrefab, effectParent);
		/*ObjectPoolManager<Transform> manager = effectParent.AddComponent<ObjectPoolManager<Transform>>();
		manager.SetManager(effectPrefab, effectParent);*/
	}
}

[Serializable]
public class Tree
{
	public AttackNode top; // 최상단 노드
	public int dataCount;

	public Tree()
	{
		top = new AttackNode(PlayerController.PlayerInput.None);
	}

	#region Insert
	public void InsertNewNode(AttackNode newNode, AttackNode curNode = null, int nodePos = 0)
	{
		if (top != null) // top이 null이 아닌 경우
		{
			InsertNewNodeProc(newNode, curNode, nodePos);
		}
		else // top이 존재하지 않을 경우
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
	#endregion

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
		if (top != null) // top이 존재하고
		{
			resultNode = FindProc(targetInput, curNode.childNodes == null ? top : curNode);
		}
		return resultNode;
	}
	#endregion
}
