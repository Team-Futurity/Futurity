using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CSSearchWindow : ScriptableObject, ISearchWindowProvider
{
	private const string NoramlAttackNodeName = "NormalAttackNode";
	private const string ChargedAttackNodeName = "ChargedAttackNode";
	private const string DashNodeName = "DashNode";

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
			new SearchTreeGroupEntry(new GUIContent("요소 생성")),
			new SearchTreeGroupEntry(new GUIContent("노드 생성"), 1),
			new SearchTreeEntry(new GUIContent("일반 공격", indentationIcon))
			{
				level = 2,
				userData = CSCommandType.NormalAttack
			},
			new SearchTreeEntry(new GUIContent("차징 공격", indentationIcon))
			{
				level = 2,
				userData = CSCommandType.ChargedAttack
			},
			new SearchTreeEntry(new GUIContent("대시", indentationIcon))
			{
				level = 2,
				userData = CSCommandType.Dash
			},
			new SearchTreeGroupEntry(new GUIContent("커맨드 그룹"), 1),
			new SearchTreeEntry(new GUIContent("공격 그룹", indentationIcon))
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
				CSNormalAttackNode normalAttackNode = graphView.CreateNode(NoramlAttackNodeName, CSCommandType.NormalAttack, localMousePosition) as CSNormalAttackNode;
				if(normalAttackNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSNormalAttackNode."); }

				graphView.AddElement(normalAttackNode);

				return true;
			case CSCommandType.ChargedAttack:
				CSChargedAttackNode chargedAttackNode = graphView.CreateNode(ChargedAttackNodeName, CSCommandType.ChargedAttack, localMousePosition) as CSChargedAttackNode;
				if (chargedAttackNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSChargedAttackNode."); }

				graphView.AddElement(chargedAttackNode);

				return true;
			case CSCommandType.Dash:
				CSDashNode dashNode = graphView.CreateNode(DashNodeName, CSCommandType.Dash, localMousePosition) as CSDashNode;
				if (dashNode == null) { FDebug.LogError("[Add Node Error] Failed to Add CSDashNode."); }

				graphView.AddElement(dashNode);

				return true;
			case Group:
				graphView.CreateGroup("CommandGroup", localMousePosition);

				return true;
			default:
				return false;
		}
	}
}
