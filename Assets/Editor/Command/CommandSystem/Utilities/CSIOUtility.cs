using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class CSIOUtility
{
	private static CommandGraphView graphView;
	private static string graphFileName;
	private const string graphFolderPath = "Assets/Editor/Command/CommandSystem/Graphs";
	private static string containerFolderPath;

	private static List<CSGroup> groups;
	private static List<CSNode> nodes;

	private static Dictionary<string, CSCommandGroupSO> createdCommandGroups;
	private static Dictionary<string, CSCommandSO> createdCommands;

	private static Dictionary<string, CSGroup> loadedGroups;
	private static Dictionary<string, CSNode> loadedNodes;

	public static void Initialize(CommandGraphView csGraphView, string graphName)
	{
		graphView = csGraphView;

		graphFileName = graphName;
		containerFolderPath = $"Assets/Data/CommandSystem/Commands/{graphFileName}";

		groups = new List<CSGroup>();
		nodes = new List<CSNode>();

		createdCommandGroups = new Dictionary<string, CSCommandGroupSO>();
		createdCommands = new Dictionary<string, CSCommandSO>();

		loadedGroups = new Dictionary<string, CSGroup>();
		loadedNodes = new Dictionary<string, CSNode>();
	}

	#region Save Mathod
	public static void Save()
	{
		CreateStaticFolders();

		GetElementsFromGraphView();

		var commandContainner = CreateAsset<CSCommandContainerSO>(containerFolderPath, graphFileName);
		commandContainner.Initialize(graphFileName);

		var graphData = CreateAsset<CSGraphSaveDataSO>(graphFolderPath, $"{graphFileName}Graph");
		graphData.Initialize(graphFileName);

		SaveGroups(graphData, commandContainner);
		SaveNodes(graphData, commandContainner);

		SaveAsset(graphData);
		SaveAsset(commandContainner);
	}

	#region Nodes
	private static void SaveNodes(CSGraphSaveDataSO graphData, CSCommandContainerSO commandContainner)
	{
		SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();
		List<string> ungroupedNodeNames = new List<string>();

		foreach (var node in nodes)
		{
			SaveNodeToGraph(node, graphData);
			SaveNodeToScriptableObject(node, commandContainner);

			if(node.NodeGroup != null)
			{
				groupedNodeNames.AddItem(node.NodeGroup.title, node.CommandName);

				continue;
			}

			ungroupedNodeNames.Add(node.CommandName);
		}

		UpateCommandConnections();

		UpateOldGroupedNodes(groupedNodeNames, graphData);
		UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
	}

	private static void UpateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, CSGraphSaveDataSO graphData)
	{
		if(graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count > 0)
		{
			foreach(KeyValuePair<string, List<string>> node in graphData.OldGroupedNodeNames)
			{
				List<string> nodesToRemove = new List<string>();

				if(currentGroupedNodeNames.ContainsKey(node.Key))
				{
					nodesToRemove = node.Value.Except(currentGroupedNodeNames[node.Key]).ToList();
				}

				foreach(string nodeToRemove in nodesToRemove)
				{
					RemoveAsset($"{containerFolderPath}/Groups/{node.Key}/Commands", nodeToRemove);
				}
			}
		}

		graphData.OldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
	}

	private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, CSGraphSaveDataSO graphData)
	{
		if(graphData.OldGroupedNodeNames != null && graphData.OldGroupedNodeNames.Count > 0)
		{
			List<string> nodesToRemoves = graphData.OldUngroupedNames.Except(currentUngroupedNodeNames).ToList();

			foreach(string node in nodesToRemoves)
			{
				RemoveAsset($"{containerFolderPath}/Global/Commands", node);
			}
		}

		graphData.OldUngroupedNames = new List<string>(currentUngroupedNodeNames);
	}

	private static void UpateCommandConnections()
	{
		foreach(var node in nodes)
		{
			var command = createdCommands[node.ID];

			for(int commandIndex = 0; commandIndex < node.NextCommands.Count; commandIndex++)
			{
				var nextCommand = node.NextCommands[commandIndex];

				if (!string.IsNullOrEmpty(nextCommand.NodeID))
				{
					command.NextCommands[commandIndex].NextCommand = createdCommands[nextCommand.NodeID];

					SaveAsset(command);
				}
			}
		}
	}

	private static void SaveNodeToScriptableObject(CSNode node, CSCommandContainerSO commandContainner)
	{
		CSCommandSO command;

		if(node.NodeGroup != null)
		{
			command = CreateAsset<CSCommandSO>($"{containerFolderPath}/Groups/{node.NodeGroup.title}/Commands", node.CommandName);

			commandContainner.CommandGroups.AddItem(createdCommandGroups[node.NodeGroup.ID], command);
		}
		else
		{
			command = CreateAsset<CSCommandSO>($"{containerFolderPath}/Global/Commands", node.CommandName);

			commandContainner.UngroupedCommands.Add(command);
		}

		command.Initialize(
			node.CommandName,
			ConvertNodeSaveDataToCommandData(node.NextCommands),
			node.CommandType,
			node.IsStartingNode()
		);

		node.SaveToCommandSO(ref command);

		createdCommands.Add(node.ID, command);

		SaveAsset(command);
	}

	private static List<CSCommandData> ConvertNodeSaveDataToCommandData(List<CSNextCommandSaveData> nextCommands)
	{
		var nextCommandList = new List<CSCommandData>();

		foreach (var command in nextCommands)
		{
			var commandData = new CSCommandData();

			nextCommandList.Add(commandData);
		}

		return nextCommandList;
	}

	private static void SaveNodeToGraph(CSNode node, CSGraphSaveDataSO graphData)
	{ 
		var nodeData = new CSNodeSaveData(node);

		graphData.Nodes.Add(nodeData);
	}
	#endregion

	#region Groups
	private static void SaveGroups(CSGraphSaveDataSO graphData, CSCommandContainerSO commandContainner)
	{
		List<string> groupNames = new List<string>();

		foreach(var group in groups)
		{
			SaveGroupToGraph(group, graphData);
			SaveGroupToScriptableObject(group, commandContainner);

			groupNames.Add(group.title);
		}

		UpdateOldGroups(groupNames, graphData);
	}

	private static void UpdateOldGroups(List<string> currentGroupNames, CSGraphSaveDataSO graphData)
	{
		if(graphData.OldGroupNames  != null && graphData.OldGroupNames.Count > 0)
		{
			List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

			foreach(var group in groupsToRemove)
			{
				RemoveFolder($"{containerFolderPath}/Group/{group}");
			}

			graphData.OldGroupNames = new List<string>(currentGroupNames);
		}
	}

	private static void RemoveFolder(string fullPath)
	{
		FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
		FileUtil.DeleteFileOrDirectory($"{fullPath}/");
	}

	private static void SaveGroupToScriptableObject(CSGroup group, CSCommandContainerSO commandContainner)
	{
		string groupName = group.title;

		string groupPath = CreateFolder($"{containerFolderPath}/Groups", groupName);
		CreateFolder(groupPath, "Commands");

		var commandGroup = CreateAsset<CSCommandGroupSO>(groupPath, groupName);

		commandGroup.Initialize(groupName);

		createdCommandGroups.Add(group.ID, commandGroup);

		commandContainner.CommandGroups.Add(commandGroup, new List<CSCommandSO>());

		SaveAsset(commandGroup);
	}

	private static void SaveGroupToGraph(CSGroup group, CSGraphSaveDataSO graphData)
	{
		var groupData = new CSGroupSaveData()
		{
			ID = group.ID,
			Name = group.title,
			Position = group.GetPosition().position
		};

		graphData.Groups.Add(groupData);
	}
	#endregion
	#endregion

	#region Load Methods
	public static void Load()
	{
		var graphData = LoadAsset<CSGraphSaveDataSO>(graphFolderPath, graphFileName);

		if(graphData == null)
		{
			EditorUtility.DisplayDialog(
				"파일을 불러올 수 없습니다!",
				$"다음 경로에서 해당 파일을 찾을 수 없습니다\n\n{graphFolderPath}/{graphFileName}\n\n올바른 파일을 위에 표시된 경로에 넣고 다시 시도 하세요.",
				"고마워요!"
			);

			return;
		}

		CommandEditorWindow.UpdateFileName(graphData.FileName);

		LoadGroups(graphData.Groups);
		LoadNodes(graphData.Nodes);
		LoadNodesConnections();
	}

	private static void LoadNodesConnections()
	{
		foreach(KeyValuePair<string, CSNode> node in loadedNodes)
		{
			var nextCommandDatas = node.Value.NextCommands;
			Port port = (Port)node.Value.outputContainer.Children().First();
			foreach (var commandData in nextCommandDatas)
			{
				if (!string.IsNullOrEmpty(commandData.NodeID))
				{
					CSNode nextNode = loadedNodes[commandData.NodeID];

					var nextNodeInputPort = (Port)nextNode.inputContainer.Children().First();

					/*Edge edge = port.ConnectTo(nextNodeInputPort);

					graphView.AddElement(edge);

					node.Value.RefreshPorts();*/

					CreateConnection(port, nextNodeInputPort);
				}
			}
		}
	}

	private static void LoadNodes(List<CSNodeSaveData> nodes)
	{
		foreach(var nodeSaveData in nodes)
		{
			var node = graphView.CreateNode(nodeSaveData.Name, nodeSaveData.CommandType, nodeSaveData.Position, false);
			
			node.LoadFromSaveData(nodeSaveData);

			node.Draw();

			graphView.AddElement(node);

			loadedNodes.Add(node.ID, node);

			if(!string.IsNullOrEmpty(nodeSaveData.GroupID))
			{
				var group = loadedGroups[nodeSaveData.GroupID];

				node.NodeGroup = group;

				group.AddElement(node);
			}

			if (nodeSaveData.IsStartingNode)
			{
				var startNodePort = (Port)graphView.startNode.outputContainer.Children().First();
				var curNodePort = (Port)node.inputContainer.Children().First();

				CreateConnection(startNodePort, curNodePort);
			}
		}
	}

	private static void LoadGroups(List<CSGroupSaveData> groups)
	{
		foreach(var groupSaveData in groups)
		{
			var group = graphView.CreateGroup(groupSaveData.Name, groupSaveData.Position);

			group.ID = groupSaveData.ID;

			loadedGroups.Add(group.ID, group);
		}
	}
	#endregion

	#region Creation Methods
	private static void CreateStaticFolders()
	{
		CreateFolder("Assets/Editor/Command/CommandSystem", "Graphs");

		string commandSystemPath = CreateFolder("Assets/Data", "CommandSystem");
		string commandsPath = CreateFolder(commandSystemPath, "Commands");

		CreateFolder(commandsPath, graphFileName);
		string globalPath = CreateFolder(containerFolderPath, "Global");
		CreateFolder(containerFolderPath, "Groups");

		CreateFolder(globalPath, "Commands");
	}

	private static string CreateFolder(string path, string folderName)
	{
		string finalPath = $"{path}/{folderName}";
		if (!AssetDatabase.IsValidFolder(finalPath))
		{
			AssetDatabase.CreateFolder(path, folderName);
		}

		return finalPath;
	}

	private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
	{
		string fullAssetName = $"{assetName}.asset";
		string fullPath = $"{path}/{fullAssetName}";
		T asset = LoadAsset<T>(path, fullAssetName);

		if (asset == null)
		{
			asset = ScriptableObject.CreateInstance<T>();

			AssetDatabase.CreateAsset(asset, fullPath);
		}		

		return asset;
	}

	public static void CreateConnection(Port outputPort, Port inputPort)
	{
		Edge edge = inputPort.ConnectTo(outputPort);

		graphView.AddElement(edge);

		outputPort.node.RefreshPorts();
	}
	#endregion

	#region Fetch Mathods
	private static void GetElementsFromGraphView()
	{
		Type groupType = typeof(CSGroup);
		graphView.graphElements.ForEach(graphElement =>
		{
			if(graphElement is CSNode node)
			{
				nodes.Add(node);

				return;
			}

			if(graphElement.GetType() == groupType)
			{
				var group = (CSGroup)graphElement;

				groups.Add(group);

				return;
			}
		});

	}

	private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
	{
		string fullPath = $"{path}/{assetName}";
		return AssetDatabase.LoadAssetAtPath<T>(fullPath);
	}

	private static void SaveAsset(UnityEngine.Object asset)
	{
		EditorUtility.SetDirty(asset);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private static void RemoveAsset(string path, string assetName)
	{
		AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
	}
	#endregion
}
