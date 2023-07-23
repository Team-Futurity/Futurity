using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CSSearchWindow : ScriptableObject, ISearchWindowProvider
{
	private CommandGraphView graphView;
	private Texture2D indentationIcon;

	public void Initialize(CommandGraphView csGraphView)
	{
		graphView = csGraphView;
		indentationIcon = new Texture2D(1, 1);
		indentationIcon.SetPixel(0, 0, Color.clear);
		indentationIcon.Apply();
	}

	public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
	{
		List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
		{
			new SearchTreeGroupEntry(new GUIContent("Create Element")),
			new SearchTreeGroupEntry(new GUIContent("Command Node"), 1),
			new SearchTreeEntry(new GUIContent("Normal Attack", indentationIcon))
			{
				level = 2,
				userData = CSCommandType.NormalAttack
			},
			new SearchTreeEntry(new GUIContent("Charged Attack", indentationIcon))
			{
				level = 2,
				userData = CSCommandType.ChargedAttack
			},
			new SearchTreeEntry(new GUIContent("Dash", indentationIcon))
			{
				level = 2,
				userData = CSCommandType.Dash
			},
			new SearchTreeGroupEntry(new GUIContent("Command Group"), 1),
			new SearchTreeEntry(new GUIContent("Attack Group", indentationIcon))
			{
				level = 2,
				userData = new Group()
			}
		};

		return searchTreeEntries;
	}

	public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
	{
		Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

		switch(searchTreeEntry.userData)
		{
			case CSCommandType.NormalAttack:
				CSNormalAttackNode normalAttackNode = graphView.CreateNode(CSCommandType.NormalAttack, localMousePosition) as CSNormalAttackNode;
				if(normalAttackNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSNormalAttackNode."); }

				graphView.AddElement(normalAttackNode);

				return true;
			case CSCommandType.ChargedAttack:
				CSChargedAttackNode chargedAttackNode = graphView.CreateNode(CSCommandType.ChargedAttack, localMousePosition) as CSChargedAttackNode;
				if (chargedAttackNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSChargedAttackNode."); }

				graphView.AddElement(chargedAttackNode);

				return true;
			case CSCommandType.Dash:
				CSDashNode dashNode = graphView.CreateNode(CSCommandType.Dash, localMousePosition) as CSDashNode;
				if (dashNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSDashNode."); }

				graphView.AddElement(dashNode);

				return true;
			case Group:
				graphView.CreateGroup("AttackGroup", localMousePosition);

				return true;
			default:
				return false;
		}
	}
}
