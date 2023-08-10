using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CSNode : Node
{
	public string ID { get; set; }
	public string CommandName { get; set; }
	public List<CSNextCommandSaveData> NextCommands { get; set; }
	public CSCommandType CommandType { get; set; }
	public CSGroup NodeGroup { get; set; }

	// Combo Data
	public float AttackLength { get; set; }
	public float AttackAngle { get; set; }
	public float AttackLengthMark { get; set; }
	public float AttackDelay { get; set; }
	public float AttackSpeed { get; set; }
	public float AttackAfterDelay { get; set; }
	public float AttackST { get; set; }
	public float AttackKnockback { get; set; }


	private Color defaultBackgroundColor;

	private CommandGraphView graphView;

	private const int MaxFloatFieldLenght = 7;

	public virtual void Initialize(string nodeName, CommandGraphView cgView, Vector2 position)
	{
		ID = Guid.NewGuid().ToString();

		CommandName = nodeName;
		CommandType = CSCommandType.NormalAttack;
		NextCommands = new List<CSNextCommandSaveData>();

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

			if(string.IsNullOrEmpty(target.value))
			{
				if(!string.IsNullOrEmpty(CommandName))
				{
					++graphView.NameErrorsAmount;
				}
			}
			else
			{
				if(string.IsNullOrEmpty(CommandName))
				{
					--graphView.NameErrorsAmount;
				}
			}

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

		// Element Initialize
		Foldout textFoldout = CSElementUtility.CreateFoldout("Command Type");
		EnumField enumField = new EnumField(CommandType);
		enumField.AddClasses(
				"ds-node__textfield",
				"ds-node__quote-textfield"
		);

		Foldout comboTextFoldout = CSElementUtility.CreateFoldout("Combo Data");
		FloatField lengthField =			new FloatField("Attack Length(공격 사거리)				|", MaxFloatFieldLenght);
		FloatField angleField =				new FloatField("Attack Angle(공격 각도)					|", MaxFloatFieldLenght);
		FloatField lengthMarkField =		new FloatField("Attack Length Mark(공격 시전지)			|", MaxFloatFieldLenght);
		FloatField delayField =				new FloatField("Attack Delay(공격 지연 시간)				|", MaxFloatFieldLenght);
		FloatField speedField =				new FloatField("Attack Speed(공격 속도)				|", MaxFloatFieldLenght);
		FloatField afterDelayField =		new FloatField("Attack After Delay(공격 후 지연 시간)		|", MaxFloatFieldLenght);
		FloatField attackSTField =			new FloatField("Attack ST(데미지 배율)					|", MaxFloatFieldLenght);
		FloatField attackKnockbackField =	new FloatField("Attack Knockback(몬스터를 밀치는 거리)		|", MaxFloatFieldLenght);
		lengthField.value = AttackLength;
		angleField.value = AttackAngle;
		lengthMarkField.value = AttackLengthMark;
		delayField.value = AttackDelay;
		speedField.value = AttackSpeed;
		afterDelayField.value = AttackAfterDelay;
		attackSTField.value = AttackST;
		attackKnockbackField.value = AttackKnockback;

		// Callbacks
		enumField.RegisterValueChangedCallback((callback) => { CommandType = (CSCommandType)callback.newValue; });
		lengthField.RegisterValueChangedCallback((callback) => { AttackLength = callback.newValue; });
		angleField.RegisterValueChangedCallback((callback) => { AttackAngle = callback.newValue; });
		lengthMarkField.RegisterValueChangedCallback((callback) => { AttackLengthMark = callback.newValue; });
		delayField.RegisterValueChangedCallback((callback) => { AttackDelay = callback.newValue; });
		speedField.RegisterValueChangedCallback((callback) => { AttackSpeed = callback.newValue; });
		afterDelayField.RegisterValueChangedCallback((callback) => { AttackAfterDelay = callback.newValue; });
		attackSTField.RegisterValueChangedCallback((callback) => { AttackST = callback.newValue; });
		attackKnockbackField.RegisterValueChangedCallback((callback) => { AttackKnockback = callback.newValue; });

		// Add
		textFoldout.Add(enumField);
		customDataContainer.Add(textFoldout);

		comboTextFoldout.Add(lengthField);
		comboTextFoldout.Add(angleField);
		comboTextFoldout.Add(lengthMarkField);
		comboTextFoldout.Add(delayField);
		comboTextFoldout.Add(speedField);
		comboTextFoldout.Add(afterDelayField);
		comboTextFoldout.Add(attackSTField);
		comboTextFoldout.Add(attackKnockbackField);
		customDataContainer.Add(comboTextFoldout);

		extensionContainer.Add(customDataContainer);

		// Output Container
		Port nextCommandsPort = this.CreatePort("다음 커맨드", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
		nextCommandsPort.userData = this;
		outputContainer.Add(nextCommandsPort);
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

	public bool IsStartingNode()
	{
		Port input = (Port)inputContainer.Children().First();

		return !input.connected;
	}

	public void SetErrorStyle(Color color)
	{
		mainContainer.style.backgroundColor = color;
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

	public void LoadFromSaveData(CSNodeSaveData saveData)
	{
		var nextCommands = CloneNodeNextCommands(saveData.NextComboNodes);

		ID = saveData.ID;
		NextCommands = nextCommands;

		AttackLength = saveData.AttackLength;
		AttackAngle = saveData.AttackAngle;
		AttackSpeed = saveData.AttackSpeed;
		AttackLengthMark = saveData.AttackLengthMark;
		AttackDelay = saveData.AttackDelay;
		AttackAfterDelay = saveData.AttackAfterDelay;
		AttackKnockback = saveData.AttackKnockback;
		AttackST = saveData.AttackST;
	}

	public void SaveToCommandSO(ref CSCommandSO so)
	{
		so.AttackLength = AttackLength;
		so.AttackAngle = AttackAngle;
		so.AttackSpeed = AttackSpeed;
		so.AttackLengthMark = AttackLengthMark;
		so.AttackDelay = AttackDelay;
		so.AttackAfterDelay = AttackAfterDelay;
		so.AttackKnockback = AttackKnockback;
		so.AttackST = AttackST;
	}
	#endregion
}
