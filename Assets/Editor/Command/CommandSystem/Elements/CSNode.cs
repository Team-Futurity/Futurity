using FMODUnity;
using PlasticGui.WorkspaceWindow.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
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
	public bool IgnoresAutoTargetMove { get; set; }
	public ColliderType AttackColliderType { get; set; }

	public List<CSAttackAssetSaveData> AttackAssets { get; set; }
	/*// Attack Effect
	public Vector3 EffectOffset { get; set; }
	public Vector3 EffectRotOffset { get; set; }
	public GameObject EffectPrefab { get; set; }
	public EffectParent AttackEffectParent { get; set; }

	// Enemy Hit Effect
	public Vector3 HitEffectOffset { get; set; }
	public Vector3 HitEffectRotOffset { get; set; }
	public GameObject HitEffectPrefab { get; set; }
	public EffectParent HitEffectParent { get; set; }*/

	// Production
	public int AnimInteger { get; set; }

	float random;
	public float ShakePower { get; set; }
	public float ShakeTime { get; set; }
	public float SlowTime { get; set; }
	public float SlowScale { get; set; }
	public float RumbleLow { get; set; }
	public float RumbleHigh { get; set; }
	public float RumbleDuration { get; set; }

	// Attack Sound
	public EventReference AttackVoice { get; set; }

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

		AttackAssets = new List<CSAttackAssetSaveData>();
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
		EnumField commandTypeField						= CreateAndRegistField("커맨드 타입				|", CommandType, textFoldout, "ds-node__textfield", "ds-node__quote-textfield");

		// combo
		Foldout comboFoldout = CSElementUtility.CreateFoldout("Combo Data");
		FloatField lengthField							= CreateAndRegistField("Attack Length(공격 사거리)				|", AttackLength, comboFoldout);
		FloatField angleField							= CreateAndRegistField("Attack Angle(공격 각도)					|", AttackAngle, comboFoldout);
		FloatField lengthMarkField						= CreateAndRegistField("Attack Length Mark(공격 시전지)			|", AttackLengthMark, comboFoldout);
		FloatField delayField							= CreateAndRegistField("Attack Delay(공격 지연 시간)				|", AttackDelay, comboFoldout);
		FloatField speedField							= CreateAndRegistField("Attack Speed(공격 속도)				|", AttackSpeed, comboFoldout);
		FloatField afterDelayField						= CreateAndRegistField("Attack After Delay(공격 후 지연 시간)		|", AttackAfterDelay, comboFoldout);
		FloatField attackSTField						= CreateAndRegistField("Attack ST(데미지 배율)					|", AttackST, comboFoldout);
		FloatField attackKnockbackField					= CreateAndRegistField("Attack Knockback(몬스터를 밀치는 힘)		|", AttackKnockback, comboFoldout);
		Toggle ignoreAutoTargetField					= CreateAndRegistField("자동 조준 이동 무시					|", IgnoresAutoTargetMove, comboFoldout);
		EnumField attackColliderTypeField				= CreateAndRegistField("공격 콜라이더 타입					|", AttackColliderType, comboFoldout);

		// Attack Asset
		Foldout assetByPassiveFoldout = CSElementUtility.CreateFoldout("Assets by Passive");
		Button addAssets = CSElementUtility.CreateButton("패시브 별 에셋 추가", () =>
		{
			Foldout foldout = CreateAttackAssetSaveDatas(null, assetByPassiveFoldout, false);
		});
		assetByPassiveFoldout.Add(addAssets);

		CSAttackAssetSaveData[] attackAssetDatas = AttackAssets.ToArray();
		foreach(var data in attackAssetDatas)
		{
			Foldout foldout = CreateAttackAssetSaveDatas(data, assetByPassiveFoldout, true);
		}

		if(AttackAssets.Count == 0) { CreateAttackAssetSaveDatas(null, assetByPassiveFoldout, false); }


		// production
		Foldout productionFoldout = CSElementUtility.CreateFoldout("Production");
		IntegerField animInteagerField					= CreateAndRegistField("애니메이션 전환값			|", AnimInteger, productionFoldout);
		FloatField shakeField							= CreateAndRegistField("커브로 흔드는 세기		|", ShakePower, productionFoldout);
		FloatField shakeTimeField						= CreateAndRegistField("흔드는 시간				|", ShakeTime, productionFoldout);
		FloatField slowTimeField						= CreateAndRegistField("슬로우 시간				|", SlowTime, productionFoldout);
		FloatField slowScaleField						= CreateAndRegistField("슬로우를 거는 세기		|", SlowScale, productionFoldout);
		FloatField rumbleLowField						= CreateAndRegistField("패드 진동 저역 세기		|", RumbleLow, productionFoldout);
		FloatField rumbleHighField						= CreateAndRegistField("패드 진동 고역 세기		|", RumbleHigh, productionFoldout);
		FloatField rumbleDurationField					= CreateAndRegistField("패드 진동 지속 시간		|", RumbleDuration, productionFoldout);

		Foldout soundFoldout = CSElementUtility.CreateFoldout("Sound");
		TextField attackVoiceField						= CreateAndRegistField("공격 목소리				|", AttackVoice.ToString(), soundFoldout);


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
		ignoreAutoTargetField.RegisterValueChangedCallback((callback) => { IgnoresAutoTargetMove = callback.newValue; });
		attackColliderTypeField.RegisterValueChangedCallback((callback) => { AttackColliderType = (ColliderType)callback.newValue; });

		/*effectOffsetField.RegisterValueChangedCallback((callback) => { EffectOffset = callback.newValue; });
		effectRotOffsetField.RegisterValueChangedCallback((callback) => { EffectRotOffset = callback.newValue; });
		effectPrefabField.RegisterValueChangedCallback((callback) => { EffectPrefab = callback.newValue as GameObject; });
		effectParentField.RegisterValueChangedCallback((callback) => { AttackEffectParent = (EffectParent)callback.newValue; });

		enemyHitEffectOffsetField.RegisterValueChangedCallback((callback) => { HitEffectOffset = callback.newValue; });
		enemyHitEffectRotOffsetField.RegisterValueChangedCallback((callback) => { HitEffectRotOffset = callback.newValue; });
		enemyHitEffectPrefabField.RegisterValueChangedCallback((callback) => { HitEffectPrefab = callback.newValue as GameObject; });
		enemyHitEffectField.RegisterValueChangedCallback((callback) => { HitEffectParent = (EffectParent)callback.newValue; });*/

		animInteagerField.RegisterValueChangedCallback((callback) => { AnimInteger = callback.newValue; });
		shakeField.RegisterValueChangedCallback((callback) => { ShakePower = callback.newValue; });
		shakeTimeField.RegisterValueChangedCallback((callback) => { ShakeTime = callback.newValue; });
		slowTimeField.RegisterValueChangedCallback((callback) => { SlowTime = callback.newValue; });
		slowScaleField.RegisterValueChangedCallback((callback) => { SlowScale = callback.newValue; });
		rumbleLowField.RegisterValueChangedCallback((callback) => { RumbleLow = callback.newValue; });
		rumbleHighField.RegisterValueChangedCallback((callback) => { RumbleHigh = callback.newValue; });
		rumbleDurationField.RegisterValueChangedCallback((callback) => { RumbleDuration = callback.newValue; });


		attackVoiceField.RegisterValueChangedCallback((callback) => { AttackVoice = EventReference.Find(callback.newValue); });

		// Add
		customDataContainer.Add(textFoldout);
		customDataContainer.Add(comboFoldout);
		customDataContainer.Add(assetByPassiveFoldout);
		/*customDataContainer.Add(attackEffectFoldout);
		customDataContainer.Add(enemyHitEffectFoldout);*/
		customDataContainer.Add(productionFoldout);
		customDataContainer.Add(soundFoldout);

		extensionContainer.Add(customDataContainer);

		// ~Output Container~ //
		Port nextCommandsPort = this.CreatePort("다음 커맨드", Orientation.Horizontal, Direction.Output, Port.Capacity.Multi);
		nextCommandsPort.userData = this;
		outputContainer.Add(nextCommandsPort);

		RefreshExpandedState();
	}

	#region Creation
	private Foldout CreateAttackAssetSaveDatas(CSAttackAssetSaveData saveData, Foldout parent, bool isReadMode)
	{
		CSAttackAssetSaveData newSaveData = saveData;
		if (saveData == null)
		{
			newSaveData = new CSAttackAssetSaveData();
			newSaveData.PartCode = AttackAssets.Count == 0 ? 0 : AttackAssets[AttackAssets.Count-1].PartCode+1;
			newSaveData.EffectOffset = Vector3.zero;
			newSaveData.EffectRotOffset = Vector3.zero;
			newSaveData.EffectPrefab = null;
			newSaveData.AttackEffectParent = EffectParent.None;
			newSaveData.HitEffectOffset = Vector3.zero;
			newSaveData.HitEffectRotOffset = Vector3.zero;
			newSaveData.HitEffectPrefab = null;
			newSaveData.HitEffectParent = EffectParent.None;
			newSaveData.AttackSound = new EventReference();
		}

		Foldout foldout = CSElementUtility.CreateFoldout(newSaveData.PartCode.ToString());

		Button deleteButton = CSElementUtility.CreateButton("X", () =>
		{
			if(AttackAssets.Count == 1) { return; }

			AttackAssets.Remove(saveData);
			parent.Remove(foldout);
		});
		foldout.Add(deleteButton);

		// Fields
		// partCode
		IntegerField partCodeField = CreateAndRegistField("PartCode", newSaveData.PartCode, foldout);

		// attack effect
		Foldout attackEffectFoldout = CSElementUtility.CreateFoldout("Attack Effect");
		Vector3Field effectOffsetField = CreateAndRegistField("이펙트 위치 오프셋		|", newSaveData.EffectOffset, attackEffectFoldout);
		Vector3Field effectRotOffsetField = CreateAndRegistField("이펙트 회전 오프셋		|", newSaveData.EffectRotOffset, attackEffectFoldout);
		ObjectField effectPrefabField = CreateAndRegistField("이펙트 프리팹			|", newSaveData.EffectPrefab, typeof(GameObject), attackEffectFoldout);
		EnumField effectParentField = CreateAndRegistField("이펙트 부모 설정			|", newSaveData.AttackEffectParent, attackEffectFoldout, "ds-node__textfield", "ds-node__quote-textfield");

		// enemy hit efffect
		Foldout enemyHitEffectFoldout = CSElementUtility.CreateFoldout("Enemy Hit Effect");
		Vector3Field enemyHitEffectOffsetField = CreateAndRegistField("적 피격 이펙트 위치 오프셋	|", newSaveData.HitEffectOffset, enemyHitEffectFoldout);
		Vector3Field enemyHitEffectRotOffsetField = CreateAndRegistField("적 피격 이펙트 회전 오프셋	|", newSaveData.HitEffectRotOffset, enemyHitEffectFoldout);
		ObjectField enemyHitEffectPrefabField = CreateAndRegistField("적 피격 이펙트 프리팹		|", newSaveData.HitEffectPrefab, typeof(GameObject), enemyHitEffectFoldout);
		EnumField enemyHitEffectField = CreateAndRegistField("적 피격 이펙트 부모 설정	|", newSaveData.HitEffectParent, enemyHitEffectFoldout, "ds-node__textfield", "ds-node__quote-textfield");

		// sound
		Foldout soundFoldout = CSElementUtility.CreateFoldout("Sound");
		TextField attackSoundField = CreateAndRegistField("공격 SE				|", newSaveData.AttackSound.ToString(), soundFoldout);

		// Set Foldout
		foldout.Add(attackEffectFoldout);
		foldout.Add(enemyHitEffectFoldout);
		foldout.Add(soundFoldout);
		parent.Add(foldout);

		// Callbacks
		partCodeField.RegisterValueChangedCallback((callback) => { foldout.text = callback.newValue.ToString(); newSaveData.PartCode = callback.newValue; });

		effectOffsetField.RegisterValueChangedCallback((callback) => { newSaveData.EffectOffset = callback.newValue; });
		effectRotOffsetField.RegisterValueChangedCallback((callback) => { newSaveData.EffectRotOffset = callback.newValue; });
		effectPrefabField.RegisterValueChangedCallback((callback) => { newSaveData.EffectPrefab = (GameObject)callback.newValue; });
		effectParentField.RegisterValueChangedCallback((callback) => { newSaveData.AttackEffectParent = (EffectParent)callback.newValue; });

		enemyHitEffectOffsetField.RegisterValueChangedCallback((callback) => { newSaveData.HitEffectOffset = callback.newValue; });
		enemyHitEffectRotOffsetField.RegisterValueChangedCallback((callback) => { newSaveData.HitEffectRotOffset = callback.newValue; });
		enemyHitEffectPrefabField.RegisterValueChangedCallback((callback) => { newSaveData.HitEffectPrefab = (GameObject)callback.newValue; });
		enemyHitEffectField.RegisterValueChangedCallback((callback) => { newSaveData.HitEffectParent = (EffectParent)callback.newValue; });

		attackSoundField.RegisterValueChangedCallback((callback) => { newSaveData.AttackSound = EventReference.Find(callback.newValue); });

		saveData = newSaveData;

		if(!isReadMode)
		{
			AttackAssets.Add(newSaveData);
		}
		

		return foldout;
	}
	#endregion

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
		IgnoresAutoTargetMove = saveData.IgnoresAutoTargetMove;
		AttackColliderType = saveData.AttackColliderType;

		AttackAssets.Clear();
		foreach (var asset in saveData.AttackAssets)
		{
			CSAttackAssetSaveData newSaveData = new CSAttackAssetSaveData();

			newSaveData.PartCode = asset.PartCode;

			newSaveData.EffectOffset = asset.EffectOffset;
			newSaveData.EffectRotOffset = asset.EffectRotOffset;
			newSaveData.EffectPrefab = asset.EffectPrefab;
			newSaveData.AttackEffectParent = asset.AttackEffectParent;

			newSaveData.HitEffectOffset = asset.HitEffectOffset;
			newSaveData.HitEffectRotOffset = asset.HitEffectRotOffset;
			newSaveData.HitEffectPrefab = asset.HitEffectPrefab;
			newSaveData.HitEffectParent = asset.HitEffectParent;

			newSaveData.AttackSound = asset.AttackSound;

			AttackAssets.Add(newSaveData);
		}
		
		/*EffectOffset = saveData.EffectOffset;
		EffectRotOffset = saveData.EffectRotOffset;
		EffectPrefab = saveData.EffectPrefab;
		AttackEffectParent = saveData.AttackEffectParent;

		HitEffectOffset = saveData.HitEffectOffset;
		HitEffectRotOffset = saveData.HitEffectOffset;
		HitEffectPrefab = saveData.HitEffectPrefab;
		HitEffectParent = saveData.HitEffectParent;*/

		AnimInteger = saveData.AnimInteger;
		ShakePower = saveData.ShakePower;
		ShakeTime = saveData.ShakeTime;
		SlowTime = saveData.SlowTime;
		SlowScale = saveData.SlowScale;
		RumbleLow = saveData.RumbleLow;
		RumbleHigh = saveData.RumbleHigh;
		RumbleDuration = saveData.RumbleDuration;

		AttackVoice = saveData.AttackVoice;
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
		so.IgnoresAutoTargetMove = IgnoresAutoTargetMove;
		so.AttackColliderType = AttackColliderType;

		so.AttackAssets.Clear();
		foreach (var asset in AttackAssets)
		{
			CSCommandAssetData data = new CSCommandAssetData();

			data.PartCode =	asset.PartCode;

			data.EffectOffset = asset.EffectOffset;
			data.EffectRotOffset = asset.EffectRotOffset;
			data.EffectPrefab = asset.EffectPrefab;
			data.AttackEffectParent = asset.AttackEffectParent;

			data.HitEffectOffset = asset.HitEffectOffset;
			data.HitEffectRotOffset = asset.HitEffectRotOffset;
			data.HitEffectPrefab = asset.HitEffectPrefab;
			data.HitEffectParent = asset.HitEffectParent;

			data.AttackSound = asset.AttackSound;

			so.AttackAssets.Add(data);
		}

		/*so.EffectOffset = EffectOffset;
		so.EffectRotOffset = EffectRotOffset;
		so.EffectPrefab = EffectPrefab;
		so.AttackEffectParent = AttackEffectParent;

		so.HitEffectOffset = HitEffectOffset;
		so.HitEffectRotOffset = HitEffectRotOffset;
		so.HitEffectPrefab = HitEffectPrefab;
		so.HitEffectParent = HitEffectParent;*/

		so.AnimInteger = AnimInteger;
		so.ShakePower = ShakePower;
		so.ShakeTime = ShakeTime;
		so.SlowTime = SlowTime;
		so.SlowScale = SlowScale;
		so.RumbleLow = RumbleLow;
		so.RumbleHigh = RumbleHigh;
		so.RumbleDuration = RumbleDuration;	

		so.AttackVoice = AttackVoice;
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

	private Toggle CreateAndRegistField(string fieldName, bool variable, Foldout category)
	{
		Toggle newField = new Toggle(fieldName);
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
