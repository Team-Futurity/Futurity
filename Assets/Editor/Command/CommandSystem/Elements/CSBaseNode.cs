using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class CSBaseNode : Node
{
	public List<CSNextCommandSaveData> NextCommands { get; set; }

	protected Color defaultBackgroundColor;

	protected CommandGraphView graphView;

	public virtual void Initialize(CommandGraphView cgView, Vector2 position)
	{
		NextCommands = new List<CSNextCommandSaveData>();

		graphView = cgView;

		defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

		SetPosition(new Rect(position, Vector2.zero));

		mainContainer.AddToClassList("ds-node__main-container");
		extensionContainer.AddToClassList("ds-node__extension-container");
	}

	public abstract void Draw();


	#region Utilities

	public void DisconnectAllPorts()
	{
		DisconnectInputPorts();
		DisconnectOutputPorts();
	}

	protected void DisconnectInputPorts()
	{
		DisconnectPorts(inputContainer);
	}

	protected void DisconnectOutputPorts()
	{
		DisconnectPorts(outputContainer);
	}

	protected void DisconnectPorts(VisualElement container)
	{
		foreach(Port port in container.Children())
		{
			if(port.connected)
			{
				graphView.DeleteElements(port.connections);
			}
		}
	}

	public void ResetStyle()
	{
		mainContainer.style.backgroundColor = defaultBackgroundColor;
	}

	public List<CSNextCommandSaveData> CloneNodeNextCommands()
	{
		List<CSNextCommandSaveData> commands = new List<CSNextCommandSaveData>();

		foreach (var command in NextCommands)
		{
			var commandData = new CSNextCommandSaveData()
			{
				NodeID = command.NodeID
			};

			commands.Add(commandData);
		}

		return commands;
	}

	public List<CSNextCommandSaveData> CloneNodeNextCommands(List<CSNextCommandSaveData> saveDatas)
	{
		List<CSNextCommandSaveData> commands = new List<CSNextCommandSaveData>();

		foreach (var command in saveDatas)
		{
			var commandData = new CSNextCommandSaveData()
			{
				NodeID = command.NodeID
			};

			commands.Add(commandData);
		}

		return commands;
	}
	#endregion
}
