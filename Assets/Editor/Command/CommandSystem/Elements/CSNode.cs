using FMODUnity;
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

	// Attack Effect
	public Vector3 EffectOffset { get; set; }
	public Vector3 EffectRotOffset { get; set; }
	public GameObject EffectPrefab { get; set; }
	public EffectParent AttackEffectParent { get; set; }

	// Enemy Hit Effect
	public Vector3 HitEffectOffset { get; set; }
	public Vector3 HitEffectRotOffset { get; set; }
	public GameObject HitEffectPrefab { get; set; }
	public EffectParent HitEffectParent { get; set; }

	// Production
	public int AnimInteger { get; set; }

	float random;
	public float RandomShakePower { get; set; }
	public float CurveShakePower { get; set; }
	public float ShakeTime { get; set; }
	public float SlowTime { get; set; }
	public float SlowScale { get; set; }

	// Attack Sound
	public EventReference AttackSound { get; set; }

	private Color defaultBackgroundColor;

	private CommandGraphView graphView;

	private const int MaxFieldLength = 8;

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
		// ~Title Container~ //
		TextField commandName = CSElementUtility.CreateTextField(CommandName, null, callback =>
		{
			TextField target = (TextField)callback.target;
			target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

			// Checking Name Error
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

		// ~Input Container~ //
		Port inputPort = this.CreatePort("이전 커맨드", Orientation.Horizontal, Direction.Input, Port.Capacity.Single);

		inputContainer.Add(inputPort);

		// ~Extensions Container~ //
		VisualElement customDataContainer = new VisualElement();

		customDataContainer.AddToClassList("ds-node__custom-data-container");

		// Element Initialize
		// default
		Foldout textFoldout = CSElementUtility.CreateFoldout("Command Type");
		EnumField commandTypeField = CreateAndRegistField("커맨드 타입				|", CommandType, textFoldout, "ds-node__textfield", "ds-node__quote-textfield");

		// combo
		Foldout comboFoldout = CSElementUtility.CreateFoldout("Combo Data");
		FloatField lengthField					= CreateAndRegistField("Attack Length(공격 사거리)				|", AttackLength, comboFoldout);
		FloatField angleField					= CreateAndRegistField("Attack Angle(공격 각도)					|", AttackAngle, comboFoldout);
		FloatField lengthMarkField				= CreateAndRegistField("Attack Length Mark(공격 시전지)			|", AttackLengthMark, comboFoldout);
		FloatField delayField					= CreateAndRegistField("Attack Delay(공격 지연 시간)				|", AttackDelay, comboFoldout);
		FloatField speedField					= CreateAndRegistField("Attack Speed(공격 속도)				|", AttackSpeed, comboFoldout);
		FloatField afterDelayField				= CreateAndRegistField("Attack After Delay(공격 후 지연 시간)		|", AttackAfterDelay, comboFoldout);
		FloatField attackSTField				= CreateAndRegistField("Attack ST(데미지 배율)					|", AttackST, comboFoldout);
		FloatField attackKnockbackField			= CreateAndRegistField("Attack Knockback(몬스터를 밀치는 거리)		|", AttackKnockback, comboFoldout);

		// attack effect
		Foldout attackEffectFoldout = CSElementUtility.CreateFoldout("Attack Effect");
		Vector3Field effectOffsetField			= CreateAndRegistField("이펙트 위치 오프셋		|", EffectOffset, attackEffectFoldout);
		Vector3Field effectRotOffsetField			= CreateAndRegistField("이펙트 회전 오프셋		|", EffectRotOffset, attackEffectFoldout);
		ObjectField effectPrefabField			= CreateAndRegistField("이펙트 프리팹			|", EffectPrefab, typeof(GameObject), attackEffectFoldout);
		EnumField effectParentField = CreateAndRegistField("이펙트 부모 설정			|", AttackEffectParent, attackEffectFoldout, "ds-node__textfield", "ds-node__quote-textfield");

		// enemy hit efffect
		Foldout enemyHitEffectFoldout = CSElementUtility.CreateFoldout("Enemy Hit Effect");
		Vector3Field enemyHitEffectOffsetField	= CreateAndRegistField("적 피격 이펙트 위치 오프셋	|", HitEffectOffset, enemyHitEffectFoldout);
		Vector3Field enemyHitEffectRotOffsetField	= CreateAndRegistField("적 피격 이펙트 회전 오프셋	|", HitEffectRotOffset, enemyHitEffectFoldout);
		ObjectField enemyHitEffectPrefabField	= CreateAndRegistField("적 피격 이펙트 프리팹		|", HitEffectPrefab, typeof(GameObject), enemyHitEffectFoldout);
		EnumField enemyHitEffectField = CreateAndRegistField("적 피격 이펙트 부모 설정	|", HitEffectParent, enemyHitEffectFoldout, "ds-node__textfield", "ds-node__quote-textfield");

		// production
		Foldout productionFoldout = CSElementUtility.CreateFoldout("Production");
		IntegerField animInteagerField			= CreateAndRegistField("애니메이션 전환값			|", AnimInteger, productionFoldout);

		FloatField randomShakeField				= CreateAndRegistField("무작위로 흔드는 정도		|", RandomShakePower, productionFoldout);
		FloatField curveShakeField				= CreateAndRegistField("커브로 흔드는 정도		|", CurveShakePower, productionFoldout);
		FloatField shakeTimeField				= CreateAndRegistField("흔드는 시간				|", ShakeTime, productionFoldout);
		FloatField slowTimeField				= CreateAndRegistField("슬로우 시간				|", SlowTime, productionFoldout);
		FloatField slowScaleField				= CreateAndRegistField("슬로우를 거는 정도		|", SlowScale, productionFoldout);

		// sound
		Foldout soundFoldout = CSElementUtility.CreateFoldout("Sound");
		TextField attackSoundField				= CreateAndRegistField("공격 SE				|", AttackSound.Path, soundFoldout);	

		// Callbacks
		commandTypeField.RegisterValueChangedCallback((callback) => { CommandType = (CSCommandType)callback.newValue; });

		lengthField.RegisterValueChangedCallback((callback) => { AttackLength = callback.newValue; });
		angleField.RegisterValueChangedCallback((callback) => { AttackAngle = callback.newValue; });
		lengthMarkField.RegisterValueChangedCallback((callback) => { AttackLengthMark = callback.newValue; });
		delayField.RegisterValueChangedCallback((callback) => { AttackDelay = callback.newValue; });
		speedField.RegisterValueChangedCallback((callback) => { AttackSpeed = callback.newValue; });
		afterDelayField.RegisterValueChangedCallback((callback) => { AttackAfterDelay = callback.newValue; });
		attackSTField.RegisterValueChangedCallback((callback) => { AttackST = callback.newValue; });
		attackKnockbackField.RegisterValueChangedCallback((callback) => { AttackKnockback = callback.newValue; });

		effectOffsetField.RegisterValueChangedCallback((callback) => { EffectOffset = callback.newValue; });
		effectRotOffsetField.RegisterValueChangedCallback((callback) => { EffectRotOffset = callback.newValue; });
		effectPrefabField.RegisterValueChangedCallback((callback) => { EffectPrefab = callback.newValue as GameObject; });
		effectParentField.RegisterValueChangedCallback((callback) => { AttackEffectParent = (EffectParent)callback.newValue; });

		enemyHitEffectOffsetField.RegisterValueChangedCallback((callback) => { HitEffectOffset = callback.newValue; });
		enemyHitEffectRotOffsetField.RegisterValueChangedCallback((callback) => { HitEffectRotOffset = callback.newValue; });
		enemyHitEffectPrefabField.RegisterValueChangedCallback((callback) => { HitEffectPrefab = callback.newValue as GameObject; });
		enemyHitEffectField.RegisterValueChangedCallback((callback) => { HitEffectParent = (EffectParent)callback.newValue; });

		animInteagerField.RegisterValueChangedCallback((callback) => { AnimInteger = callback.newValue; });
		randomShakeField.RegisterValueChangedCallback((callback) => { RandomShakePower = callback.newValue; });
		curveShakeField.RegisterValueChangedCallback((callback) => { CurveShakePower = callback.newValue; });
		shakeTimeField.RegisterValueChangedCallback((callback) => { ShakeTime = callback.newValue; });
		slowTimeField.RegisterValueChangedCallback((callback) => { SlowTime = callback.newValue; });
		slowScaleField.RegisterValueChangedCallback((callback) => { SlowScale = callback.newValue; });

		attackSoundField.RegisterValueChangedCallback((callback) => { AttackSound = EventReference.Find(callback.newValue); });

		// Add
		customDataContainer.Add(textFoldout);
		customDataContainer.Add(comboFoldout);
		customDataContainer.Add(attackEffectFoldout);
		customDataContainer.Add(enemyHitEffectFoldout);
		customDataContainer.Add(productionFoldout);
		customDataContainer.Add(soundFoldout);

		extensionContainer.Add(customDataContainer);

		// ~Output Container~ //
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

		return input.connected && input.userData is CSStartNode;
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

		EffectOffset = saveData.EffectOffset;
		EffectRotOffset = saveData.EffectRotOffset;
		EffectPrefab = saveData.EffectPrefab;
		AttackEffectParent = saveData.AttackEffectParent;

		HitEffectOffset = saveData.HitEffectOffset;
		HitEffectRotOffset = saveData.HitEffectOffset;
		HitEffectPrefab = saveData.HitEffectPrefab;
		HitEffectParent = saveData.HitEffectParent;

		AnimInteger = saveData.AnimInteger;
		RandomShakePower = saveData.RandomShakePower;
		CurveShakePower = saveData.CurveShakePower;
		ShakeTime = saveData.ShakeTime;
		SlowTime = saveData.SlowTime;
		SlowScale = saveData.SlowScale;

		AttackSound = saveData.AttackSound;
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

		so.EffectOffset = EffectOffset;
		so.EffectRotOffset = EffectRotOffset;
		so.EffectPrefab = EffectPrefab;
		so.AttackEffectParent = AttackEffectParent;

		so.HitEffectOffset = HitEffectOffset;
		so.HitEffectRotOffset = HitEffectRotOffset;
		so.HitEffectPrefab = HitEffectPrefab;
		so.HitEffectParent = HitEffectParent;

		so.AnimInteger = AnimInteger;
		so.RandomShakePower = RandomShakePower;
		so.CurveShakePower = CurveShakePower;
		so.ShakeTime = SlowTime;
		so.SlowTime = SlowTime;
		so.SlowScale = SlowScale;

		so.AttackSound = AttackSound;
	}

	private FloatField CreateAndRegistField(string fieldName, float variable, Foldout category)
	{
		FloatField newField = new FloatField(fieldName, MaxFieldLength);
		newField.value = variable;

		category.Add(newField);

		return newField;
	}

	private IntegerField CreateAndRegistField(string fieldName, int variable, Foldout category)
	{
		IntegerField newField = new IntegerField(fieldName, MaxFieldLength);
		newField.value = variable;

		category.Add(newField);

		return newField;
	}

	private ObjectField CreateAndRegistField(string fieldName, UnityEngine.Object variable, Type objectType, Foldout category)
	{
		ObjectField newField = new ObjectField(fieldName);
		newField.objectType = objectType;
		newField.value = variable;
		newField.allowSceneObjects = false;

		category.Add(newField);

		return newField;
	}

	private Vector3Field CreateAndRegistField(string fieldName, Vector3 variable, Foldout category)
	{
		Vector3Field newField = new Vector3Field(fieldName);
		newField.value = variable;

		category.Add(newField);

		return newField;
	}

	private TextField CreateAndRegistField(string fieldName, string variable, Foldout category)
	{
		TextField newField = new TextField(fieldName);
		newField.value = variable;

		category.Add(newField);

		return newField;
	}

	private EnumField CreateAndRegistField(string fieldName, Enum variable, Foldout category, params string[] classNames)
	{
		EnumField newField = new EnumField(fieldName, variable);
		newField.value = variable;

		newField.AddClasses(classNames);

		category.Add(newField);

		return newField;
	}
	#endregion
}
