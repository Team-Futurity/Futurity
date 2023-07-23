using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CSNode : Node
{
	public string ID { get; set; }
	public string CommandName { get; set; }
	public CSCommandType CommandType { get; set; }
	public CSGroup NodeGroup { get; set; }

	private Color defaultBackgroundColor;

	private CommandGraphView graphView;

	public virtual void Initialize(CommandGraphView cgView, Vector2 position)
	{
		ID = Guid.NewGuid().ToString();

		CommandName = "CommandName";
		CommandType = CSCommandType.NormalAttack;

		graphView = cgView;

		defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

		SetPosition(new Rect(position, Vector2.zero));

		mainContainer.AddToClassList("ds-node__main-container");
		extensionContainer.AddToClassList("ds-node__extension-container");
	}

	public virtual void Draw()
	{
		// Title Container
		TextField commandName = CSElementUtility.CreateTextField(CommandName, null, callback =>
		{
			TextField target = (TextField)callback.target;
			target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
			if(NodeGroup == null)
			{
				graphView.RemoveUngroupedNode(this);

				CommandName = target.value;

				graphView.AddUngroupedNode(this);
			}
			else
			{
				CSGroup curGroup = NodeGroup;

				graphView.RemoveGroupedNode(this, curGroup);

				CommandName = target.value;
				
				graphView.AddGroupedNode(this, curGroup);
			}
		});

		commandName.AddClasses(
				"ds-node__textfield",
				"ds-node__filename-textfield",
				"ds-node__textfield__hidden"
		);

		titleContainer.Insert(0, commandName);
		//titleContainer.Add(commandName);

		// Input Container
		Port inputPort = this.CreatePort("이전 커맨드", Orientation.Horizontal, Direction.Input, Port.Capacity.Single);

		inputContainer.Add(inputPort);

		// Extensions Container
		VisualElement customDataContainer = new VisualElement();

		customDataContainer.AddToClassList("ds-node__custom-data-container");

		Foldout textFoldout = CSElementUtility.CreateFoldout("Command Type");
		EnumField enumField = new EnumField(CommandType);

		enumField.AddClasses(
				"ds-node__textfield",
				"ds-node__quote-textfield"
		);

		textFoldout.Add(enumField);
		customDataContainer.Add(textFoldout);

		extensionContainer.Add(customDataContainer);

		// Output Container
		Port nextCommands = this.CreatePort("다음 커맨드", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
		outputContainer.Add(nextCommands);
	}

	#region Overrided
	public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
	{
		evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
		evt.menu.AppendAction("Disconnect output Ports", actionEvent => DisconnectOutputPorts());

		base.BuildContextualMenu(evt);


	}
	#endregion


	#region Utilities
	public void DisconnectAllPorts()
	{
		DisconnectInputPorts();
		DisconnectOutputPorts();
	}

	private void DisconnectInputPorts()
	{
		DisconnectPorts(inputContainer);
	}

	private void DisconnectOutputPorts()
	{
		DisconnectPorts(outputContainer);
	}

	private void DisconnectPorts(VisualElement container)
	{
		foreach(Port port in container.Children())
		{
			if(port.connected)
			{
				graphView.DeleteElements(port.connections);
			}
		}
	}

	public void SetErrorStyle(Color color)
	{
		mainContainer.style.backgroundColor = color;
	}

	public void ResetStyle()
	{
		mainContainer.style.backgroundColor = defaultBackgroundColor;
	}
	#endregion
}
