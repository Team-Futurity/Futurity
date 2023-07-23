using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CommandGraphView : GraphView
{
	private CSSearchWindow searchWindow;
	private CommandEditorWindow editorWindow;

	private Dictionary<string, CSNodeErrorData> ungroupedNodes;
	private Dictionary<string, CSGroupErrorData> groups;
	private Dictionary<(Group, string), CSNodeErrorData> groupedNodes;

	private int repeatedNamesAmount;
	public int RepeatedNamesAmount
	{
		get { return repeatedNamesAmount; }
		set 
		{ 
			repeatedNamesAmount = value; 

			if(repeatedNamesAmount == 0)
			{
				editorWindow.SetEnableSaving(true);
			}
			else if(repeatedNamesAmount == 1) 
			{
				editorWindow.SetEnableSaving(false);
			}
		}
	}

	public CommandGraphView(CommandEditorWindow editorWindow)
	{
		this.editorWindow = editorWindow;

		ungroupedNodes = new Dictionary<string, CSNodeErrorData>();
		groups = new Dictionary<string, CSGroupErrorData>();
		groupedNodes = new Dictionary<(Group, string), CSNodeErrorData>();

		// Control
		AddSearchWindow();
		AddManipulators();
		AddGridBackground();

		// Callbacks
		OnElementsDeleted();
		OnGroupElementsAdded();
		OnGroupElementRemoved();
		OnGroupRenamed();

		// Style
		AddStyles();
	}

	#region Overrided Methods
	public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
	{
		List<Port> compatiblePorts = new List<Port>();

		ports.ForEach(port =>
		{
			if(startPort == port)
			{
				return;
			}

			if(startPort.node == port.node)
			{
				return;
			}

			if(startPort.direction == port.direction)
			{
				return;
			}

			compatiblePorts.Add(port);
		});

		return compatiblePorts;
	}
	#endregion

	#region Mainpulators
	private void AddManipulators()
	{
		// 줌인 줌아웃 기능
		SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

		// 버튼 추가 기능
		this.AddManipulator(CreateNodeContextualMenu("Add Node (Normal Attack)", CSCommandType.NormalAttack));
		this.AddManipulator(CreateNodeContextualMenu("Add Node (Charged Attack)", CSCommandType.ChargedAttack));
		this.AddManipulator(CreateNodeContextualMenu("Add Node (Dash)", CSCommandType.Dash));

		// 선택된 요소 드래그 이동
		// RectangleSelector보다 나중에 Add되면 정상 동작하지 않음
		this.AddManipulator(new SelectionDragger());

		// 그래프 요소 선택 기능
		this.AddManipulator(new RectangleSelector());

		// 드래그 기능
		this.AddManipulator(new ContentDragger());

		this.AddManipulator(CreateGroupContextMenu());
	}

	private IManipulator CreateGroupContextMenu()
	{
		ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
				menuEvent =>
					menuEvent.menu.AppendAction
						(
							"Add Group",
							actionEvent => CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))
						)
			);

		return contextualMenuManipulator;
	}

	private IManipulator CreateNodeContextualMenu(string actionTitle, CSCommandType commandType)
	{
		ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
				menuEvent => 
					menuEvent.menu.AppendAction
						(
							actionTitle, 
							actionEvent => AddElement(CreateNode(commandType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
						)
			);

		return contextualMenuManipulator;
	}
	#endregion

	#region Elements Creation
	public CSGroup CreateGroup(string title, Vector2 localMousePosition)
	{
		CSGroup group = new CSGroup(title, localMousePosition);

		group.SetPosition(new Rect(localMousePosition, Vector2.zero));

		AddGroup(group);
		AddElement(group);

		foreach(var selectedElement in selection)
		{
			if((selectedElement is CSNode))
			{
				var node = (CSNode)selectedElement;

				group.AddElement(node);
			}
		}

		return group;
	}

	public CSNode CreateNode(CSCommandType commandType, Vector2 position)
	{
		Type nodeType = Type.GetType($"CS{commandType}Node");

		CSNode node = (CSNode)Activator.CreateInstance(nodeType);

		node.Initialize(this, position);
		node.Draw();

		AddUngroupedNode(node);

		AddElement(node);

		return node;
	}
	#endregion

	#region Repeated Elements
	public void AddUngroupedNode(CSNode node)
	{
		string nodeName = node.CommandName.ToLower();

		if(!ungroupedNodes.ContainsKey(nodeName))
		{
			CSNodeErrorData nodeErrorData = new CSNodeErrorData();

			nodeErrorData.Nodes.Add(node);

			ungroupedNodes.Add(nodeName, nodeErrorData);

			return;
		}

		var ungroupedNodeData = ungroupedNodes[nodeName];

		ungroupedNodeData.Nodes.Add(node);

		Color errorColor = ungroupedNodeData.ErrorData.Color;

		node.SetErrorStyle(errorColor);

		if (ungroupedNodeData.Nodes.Count == 2)
		{
			RepeatedNamesAmount++;
			ungroupedNodeData.Nodes[0].SetErrorStyle(errorColor);
		}
	}

	public void RemoveUngroupedNode(CSNode node)
	{
		var nodeName = node.CommandName.ToLower();

		var ungroupedNodeList = ungroupedNodes[nodeName].Nodes;
		ungroupedNodeList.Remove(node);

		node.ResetStyle();

		if(ungroupedNodeList.Count == 1)
		{
			RepeatedNamesAmount--;
			ungroupedNodeList[0].ResetStyle();
		}
		else if(ungroupedNodeList.Count == 0)
		{
			ungroupedNodes.Remove(nodeName);
		}
	}

	public void AddGroupedNode(CSNode node, CSGroup group)
	{
		string nodeName = node.CommandName.ToLower();
		
		node.NodeGroup = group;

		if(!groupedNodes.ContainsKey((group, nodeName)))
		{
			var csNodeErrorData = new CSNodeErrorData();

			csNodeErrorData.Nodes.Add(node);
			groupedNodes.Add((group, nodeName), csNodeErrorData);

			return;
		}

		var nodeErrorData = groupedNodes[(group, nodeName)];

		nodeErrorData.Nodes.Add(node);

		Color errorColor = nodeErrorData.ErrorData.Color;

		node.SetErrorStyle(errorColor);

		if(nodeErrorData.Nodes.Count == 2)
		{
			RepeatedNamesAmount++;
			nodeErrorData.Nodes[0].SetErrorStyle(errorColor);
		}
	}

	public void RemoveGroupedNode(CSNode node, Group group)
	{
		var nodeName = node.CommandName.ToLower();

		var groupedNodeList = groupedNodes[(group, nodeName)].Nodes;

		node.NodeGroup = null;

		groupedNodeList.Remove(node);

		node.ResetStyle();

		if (groupedNodeList.Count == 1)
		{
			RepeatedNamesAmount--;
			groupedNodeList[0].ResetStyle();
		}
		else if (groupedNodeList.Count == 0)
		{
			groupedNodes.Remove((group, nodeName));
		}
	}

	public void AddGroup(CSGroup group)
	{
		string groupName = group.title.ToLower();

		if (!groups.ContainsKey(groupName))
		{
			var groupErrorData = new CSGroupErrorData();

			groupErrorData.Groups.Add(group);

			groups.Add(groupName, groupErrorData);

			return;
		}

		var groupNodeData = groups[groupName];

		groupNodeData.Groups.Add(group);

		Color errorColor = groupNodeData.ErrorData.Color;

		group.SetErrorStyle(errorColor);

		if (groupNodeData.Groups.Count == 2)
		{
			RepeatedNamesAmount++;
			groupNodeData.Groups[0].SetErrorStyle(errorColor);
		}
	}

	public void RemoveGroup(CSGroup group)
	{
		var oldGroupName = group.OldTitle.ToLower();
		var groupNodeData = groups[oldGroupName].Groups;

		groupNodeData.Remove(group);

		group.ResetStyle();

		if (groupNodeData.Count == 1)
		{
			RepeatedNamesAmount--;
			groupNodeData[0].ResetStyle();
		}
		else if (groupNodeData.Count == 0)
		{
			groups.Remove(oldGroupName);
		}
	}
	#endregion

	#region Callbacks
	private void OnElementsDeleted()
	{
		deleteSelection = (operationName, askUser) =>
		{
			Type groupType = typeof(CSGroup);
			Type edgeType = typeof(Edge);

			List<CSNode> nodesToDelete = new List<CSNode>();
			List<CSGroup> groupsToDelete = new List<CSGroup>();
			List<Edge> edgesToDelete = new List<Edge>();

			foreach(var element in selection)
			{
				if(element is CSNode node)
				{
					nodesToDelete.Add(node);
				}
				else if(element.GetType() == edgeType)
				{
					var edge = (Edge)element;
					edgesToDelete.Add(edge);
				}
				else if(element.GetType() == groupType)
				{
					var group = (CSGroup)element;
					groupsToDelete.Add(group);
				}
			}

			foreach (var group in groupsToDelete)
			{
				var groupNodes = new List<CSNode>();

				foreach(var groupElement in group.containedElements)
				{
					if(groupElement is CSNode node)
					{
						groupNodes.Add(node);
					}
				}

				group.RemoveElements(groupNodes);
				RemoveGroup(group);
				RemoveElement(group);
			}

			foreach (var node in nodesToDelete)
			{
				if(node.NodeGroup != null)
				{
					node.NodeGroup.RemoveElement(node);
				}

				RemoveUngroupedNode(node);

				node.DisconnectAllPorts();

				RemoveElement(node);
			}

			DeleteElements(edgesToDelete);
		};
	}

	private void OnGroupElementsAdded()
	{
		elementsAddedToGroup = (group, elements) =>
		{
			foreach (var element in elements)
			{
				if (element is CSNode node)
				{
					CSGroup nodeGroup = group as CSGroup;
					RemoveUngroupedNode(node);
					AddGroupedNode(node, nodeGroup);
				}
			}
		};
	}	

	private void OnGroupElementRemoved()
	{
		elementsRemovedFromGroup = (group, elements) =>
		{
			foreach (var element in elements)
			{
				if (element is CSNode node)
				{
					RemoveGroupedNode(node, group);
					AddUngroupedNode(node);
				}
			}
		};
	}

	private void OnGroupRenamed()
	{
		groupTitleChanged = (group, newTitle) =>
		{
			var csGroup = (CSGroup)group;

			csGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

			RemoveGroup(csGroup);

			csGroup.OldTitle = csGroup.title;

			AddGroup(csGroup);
		};
	}
	#endregion

	#region Elements Addition
	private void AddSearchWindow()
	{
		if (searchWindow == null)
		{
			searchWindow = ScriptableObject.CreateInstance<CSSearchWindow>();

			searchWindow.Initialize(this);
		}

		nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
	}

	private void AddGridBackground()
	{
		GridBackground gridBackground = new GridBackground();

		gridBackground.StretchToParentSize();

		Insert(0, gridBackground);
	}

	private void AddStyles()
	{
		this.AddStyleSheets(
			"CommandSystem/CommandGraphViewStyles.uss",
			"CommandSystem/CSNodeStyles.uss"
		);
	}
	#endregion

	#region Utilites
	public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
	{
		Vector2 worldMousePosition = mousePosition;

		if(isSearchWindow)
		{
			worldMousePosition -= editorWindow.position.position;
		}

		Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

		return localMousePosition;
	}
	#endregion
}
