using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : UnitFSM<PlayerController>, IFSM
{
	// Constants
	public readonly string EnemyTag = "Enemy";
	public readonly string ComboAttackAnimaKey = "ComboParam";
	public readonly string ChargedAttackAnimaKey = "ChargingParam";
	public readonly string IsAttackingAnimKey = "IsAttacking";
	public const int NullState = -1;

	[Header("[수치 조절]────────────────────────────────────────────────────────────────────────────────────────────")]

	// attack
	[Space(2)]
	[Header("콤보")]
	public CommandTree commandTree;

	[Space(5)]
	[Header("자동 조준")]
	[Tooltip("자동 조준 거리(cm)")]public float autoLength;
	[Tooltip("자동 조준 각도(육십분법)")]public float autoAngle;
	[Tooltip("움직일 시간")]public float moveTime;
	[Tooltip("멈춰설 거리(cm)")]public float moveMargin;

	// move
	[Space(5)]
	[Header("이동")]
	[Tooltip("회전하는 속도")]
	public float rotatePower;
	[Tooltip("멈춰설 벽과의 거리")]
	public float stopDistance;
	[Range(0, 90)] public float stopAngle;

	// dash
	[Space(5)]
	[Header("대시. 런타임 변경 불가")]
	public float dashCoolTime;
	public GameObject dashEffect;
	public Transform dashPos;
	public ObjectPoolManager<Transform> dashPoolManager;
	public int maxDashCount;

	// hit
	[Space(5)]
	[Header("피격. 런타임 변경 불가")]
	public float hitCoolTime;

	[Space(15)]
	[Header("[디버깅용]─────────────────────────────────────────────────────────────────────────────────────────────")]

	// move
	[Space(2)]
	[Header("이동 관련")]
	public Vector3 moveDir;
	public Vector3 lastMoveDir;

	// dash
	[Space(5)]
	[Header("대시 관련")]
	public bool dashCoolTimeIsEnd = false;
	public bool comboIsEnd = false;
	public int currentDashCount;

	// input
	[Space(5)]
	[Header("입력 관련")]
	public bool specialIsReleased = false;
	public bool moveIsPressed = false;
	private bool comboIsLock = false;

	// attack
	[Space(5)]
	[Header("공격 관련")]
	public PlayerInputEnum curCombo;
	public PlayerInputEnum nextCombo;
	public AttackNode curNode;
	public AttackNode firstBehaiviorNode;
	public PlayerState currentAttackState;
	[HideInInspector] public string currentAttackAnimKey;

	// hit
	[Space(5)]
	[Header("피격 관련")]
	public bool hitCoolTimeIsEnd = false;

	[Space(15)]
	[Header("[최초 1회 할당]──────────────────────────────────────────────────────────────────────────────────────────")]

	// reference
	[Space(2)]
	[Header("References")]
	public GameObject glove;
	public GameObject rushGlove;
	public Player playerData;
	public CommandTreeLoader commandTreeLoader;
	public SpecialMoveController activePartController;
	public ComboGaugeSystem comboGaugeSystem;
	public HitCountSystem hitCountSystem;
	public ColliderChanger attackColliderChanger;
	public ColliderChanger autoTargetColliderChanger;
	public CapsuleCollider basicCollider;
	public EffectController effectController;
	public EffectDatas effectSO;
	//public BuffProvider buffProvider;
	public RootMotionContoller rmController;
	public PlayerAnimationEvents playerAnimationEvents;
	public PlayerCameraEffect cameraEffect;
	public PartSystem partSystem;
	[HideInInspector] public CameraFollowTarget followTarget;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	/*[HideInInspector] public TruncatedCapsuleCollider attackCollider;
	[HideInInspector] public TruncatedCapsuleCollider autoTargetCollider;*/
	private WaitForSeconds dashCoolTimeWFS;
	private WaitForSeconds hitCoolTimeWFS;

	// event
	[HideInInspector] public UnityEvent<PlayerState> nextStateEvent;
	[HideInInspector] public InputAction moveAction;
	[HideInInspector] public UnityEvent<string> attackEndEvent;

	// Temporary
	[Serializable]
	public struct EffectData
	{
		public Transform effectPos;
		public GameObject effect;
	}
	[Space(5)]
	[Header("돌진 이펙트")]
	public List<EffectData> rushEffects;
	public ObjectPoolManager<Transform> rushObjectPool;
	public ObjectPoolManager<Transform> rushObjectPool2;

	// sound 
	[Space(5)]
	[Header("사운드")]
	public FMODUnity.EventReference dash;
	public FMODUnity.EventReference hitMelee;
	public FMODUnity.EventReference hitRanged;

	// etc
	[HideInInspector] public bool activePartIsActive; // 액티브 부품이 사용가능한지

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody>();

		// effect
		effectController = ECManager.Instance.GetEffectManager(effectSO);

		// ReferenceCheck
		List<string> msgs;
		if(!CheckReference(out msgs))
		{
			DebugLogger.PrintDebugErros(msgs, typeof(PlayerController), DebugTypeEnum.Error);
			return;
		}
		else
		{
			DebugLogger.PrintDebugErros(msgs, typeof(PlayerController), DebugTypeEnum.Log);
		}

		// Animator Init
		animator.SetInteger(ComboAttackAnimaKey, NullState);
		animator.SetInteger(ChargedAttackAnimaKey, NullState);
		rmController.SetStopDistance(moveMargin * MathPlus.cm2m);

		// UnitFSM Init
		SetFSM();

		UnitState<PlayerController> astate = null;
		GetState(PlayerState.AttackAfterDelay, ref astate);
		nextStateEvent.AddListener((state) => { ((PlayerAttackAfterDelayState)astate).NextAttackState(unit, state); });

		// Attack Init
		commandTree = commandTreeLoader.GetCommandTree();
		curNode = commandTree.Top;
		nextCombo = PlayerInputEnum.None;
		firstBehaiviorNode = null;
		//comboTree.SetTree(comboTree.top, null);

		// Glove Init
		SetGauntlet(false);

		// dash
		dashPoolManager = new ObjectPoolManager<Transform>(dashEffect, gameObject);
		dashCoolTimeWFS = new WaitForSeconds(dashCoolTime);
		currentDashCount = maxDashCount;
		StartCoroutine(DashDelayCoroutine());

		// hit
		hitCoolTimeWFS = new WaitForSeconds(hitCoolTime);
		StartCoroutine(HitDelayCoroutine());

		followTarget = gameObject.GetComponentInChildren<CameraFollowTarget>();
	}

	public void SetFSM()
	{
		unit = this;
		SetUp(PlayerState.Idle);
	}

	#region Input
	public PlayerInputData GetInputData(PlayerInputEnum input, bool isProcess, params string[] additionalDatas)
	{
		PlayerInputData data;

		string returnValue = $"Input_{(int)input}_";

		returnValue += isProcess ? "T_" : "F_";

		for(int i = 0; i < additionalDatas.Length; i++)
		{
			if (additionalDatas[i] != null)
			{
				returnValue += $"{additionalDatas[i]}_";
			}
		}

		returnValue += "End";

		data.inputMsg = returnValue;
		data.inputState = input;

		return data;
	}

	public PlayerInputData MoveProcess(InputAction.CallbackContext context)
	{
		Vector3 input = context.ReadValue<Vector3>();

		moveDir = new Vector3(input.x, 0f, input.y);

		if(moveDir != Vector3.zero)
		{
			lastMoveDir = new Vector3(input.x, 0f, input.y);
		}
		
		moveAction = context.action;

		moveIsPressed = (!context.started || context.performed) ^ context.canceled && moveDir != Vector3.zero;

		if (IsCurrentState(PlayerState.BasicSM)) { return GetInputData(PlayerInputEnum.Move, false, moveDir.ToString()); }

		// 예외처리
		if (!IsCurrentState(PlayerState.Idle))
		{
			// 돌진 중 이동 기능
			if (IsAttackProcess())
			{
				if (IsCurrentState(PlayerState.ChargedAttack))
				{
					animator.SetTrigger("MoveDuringRushPreparing");
					AddSubState(PlayerState.Move);
					return GetInputData(PlayerInputEnum.Move, true, moveDir.ToString(), "SubState");
				}
			}
			return GetInputData(PlayerInputEnum.Move, false, moveDir.ToString());
		}

		if (playerData.isStun || !hitCoolTimeIsEnd)
		{
			return GetInputData(PlayerInputEnum.Move, false, moveDir.ToString()); ;
		}

		// 이동 기능
		if (!IsCurrentState(PlayerState.Move))
		{
			ChangeState(PlayerState.Move);
			return GetInputData(PlayerInputEnum.Move, true, moveDir.ToString(), "State");
		}

		return GetInputData(PlayerInputEnum.Move, false, moveDir.ToString());
	}

	public PlayerInputData DashProcess(InputAction.CallbackContext context)
	{
		if (IsCurrentState(PlayerState.Hit) || IsCurrentState(PlayerState.Death) || IsCurrentState(PlayerState.BasicSM) || playerData.isStun || !hitCoolTimeIsEnd || currentDashCount <= 0) 
		{ 
			return GetInputData(PlayerInputEnum.Dash, false); 
		}

		if (!IsCurrentState(PlayerState.Dash))
		{
			ChangeState(PlayerState.Dash);
			return GetInputData(PlayerInputEnum.Dash, true);
		}
		else
		{
			ChangeState(PlayerState.Idle);
			ChangeState(PlayerState.Dash);
		}

		return GetInputData(PlayerInputEnum.Dash, false);
	}

	// Normal Attack
	public PlayerInputData NAProcess(InputAction.CallbackContext context)
	{
		// 피격 중이거나, 스턴 상태면 리턴
		if (playerData.isStun || !hitCoolTimeIsEnd || IsCurrentState(PlayerState.Death) || IsCurrentState(PlayerState.BasicSM)) { return GetInputData(PlayerInputEnum.NormalAttack, false); }

		// Idle, Move, Attack 관련 State가 아니면 리턴
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true) && IsCurrentState(PlayerState.ChargedAttack)) { return GetInputData(PlayerInputEnum.NormalAttack, false); }

		// AfterDelay나 다른 스테이트(Idle, Move)라면
		if (!IsAttackProcess(true))
		{
			StartNextComboAttack(PlayerInputEnum.NormalAttack, PlayerState.NormalAttack);

			return GetInputData(PlayerInputEnum.NormalAttack, true, currentAttackState.ToString(), curNode.name);
		}
		else // 공격 중이라면
		{
			SetNextCombo(PlayerInputEnum.NormalAttack);
			return GetInputData(PlayerInputEnum.NormalAttack, true, "Queueing", FindInput(PlayerInputEnum.NormalAttack).name);
		}
	}

	// Special Attack
	public PlayerInputData SAProcess(InputAction.CallbackContext context)
	{
		// 피격 중이거나, 스턴 상태면 리턴
		if (playerData.isStun || !hitCoolTimeIsEnd || IsCurrentState(PlayerState.Death) || IsCurrentState(PlayerState.BasicSM)) { return GetInputData(PlayerInputEnum.SpecialAttack, false); }

		// Idle, Move, Attack 관련 State가 아니면 리턴
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return GetInputData(PlayerInputEnum.SpecialAttack, false); }

		var state = curCombo != PlayerInputEnum.NormalAttack ? PlayerState.ChargedAttack : PlayerState.NormalAttack;
		if (context.started)
		{
			if (!IsAttackProcess(true))
			{
				//StartNextComboAttack(PlayerInputEnum.SpecialAttack, state);
				return GetInputData(PlayerInputEnum.SpecialAttack, true, state.ToString(), state == PlayerState.NormalAttack ? curNode.name : "Pressed");
			}
			else
			{
				if (firstBehaiviorNode.command == PlayerInputEnum.NormalAttack)
				{
					SetNextCombo(PlayerInputEnum.SpecialAttack);
					return GetInputData(PlayerInputEnum.SpecialAttack, true, "Queueing");
				}
			}
		}
		else
		{
			if (context.canceled && currentAttackState == PlayerState.ChargedAttack)
			{
				specialIsReleased = true;
				return GetInputData(PlayerInputEnum.SpecialAttack, true, state.ToString(), "Released");
			}
		}
		
		return GetInputData(PlayerInputEnum.SpecialAttack, false);
	}

	// Special Move
	public PlayerInputData SMProcess(InputAction.CallbackContext context)
	{
		//if (!activePartIsActive) { return GetInputData(PlayerInputEnum.SpecialMove, false); }

		if (comboGaugeSystem.CurrentGauge < 100) { return GetInputData(PlayerInputEnum.SpecialMove, false); }
		if (IsCurrentState(PlayerState.Death) || IsCurrentState(PlayerState.BasicSM)) { return GetInputData(PlayerInputEnum.SpecialMove, false); }

		activePartController.RunActivePart(this, playerData, SpecialMoveType.Basic);
		return GetInputData(PlayerInputEnum.SpecialAttack, true, SpecialMoveType.Basic.ToString());
	}
	#endregion

	#region legacy input
	/*	public void OnSpecialMove(InputAction.CallbackContext context)
		{
			if(!context.started || !activePartIsActive) { return; }

			activePartController.RunActivePart(this, playerData, ActivePartType.Basic);
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			// Input
			Vector3 input = context.ReadValue<Vector3>();

			if (input == null) { return; }
			moveDir = new Vector3(input.x, 0f, input.y);
			moveAction = context.action;

			moveIsPressed = (!context.started || context.performed) ^ context.canceled && moveDir != Vector3.zero;

			// 예외처리
			if (!IsCurrentState(PlayerState.Idle))
			{
				// 돌진 중 이동 기능
				if (IsAttackProcess())
				{
					if (IsCurrentState(PlayerState.ChargedAttack))
					{
						animator.SetTrigger("MoveDuringRushPreparing");
						AddSubState(PlayerState.Move);
					}
				}
				return;
			}

			if(playerData.isStun || !hitCoolTimeIsEnd)
			{
				return;
			}

			// 이동 기능
			if (!IsCurrentState(PlayerState.Move))
			{
				ChangeState(PlayerState.Move);
			}
		}

		public void OnDash(InputAction.CallbackContext context)
		{
			if (IsCurrentState(PlayerState.Hit) || playerData.isStun || !dashCoolTimeIsEnd || !hitCoolTimeIsEnd) { return; }

			if (context.performed)
			{
				if (!IsCurrentState(PlayerState.Dash))
				{
					ChangeState(PlayerState.Dash);
				}
			}
		}

		public void OnNormalAttack(InputAction.CallbackContext context)
		{
			// 입력이 되지 않았으면(Pressed 시점이 아니면) 리턴
			if (!context.started) { return; }

			FDebug.Log("Normal");

			// 피격 중이거나, 스턴 상태면 리턴
			if (playerData.isStun || !hitCoolTimeIsEnd) { return; }

			// Idle, Move, Attack 관련 State가 아니면 리턴
			if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return; }

			// AfterDelay나 다른 스테이트(Idle, Move)라면
			if (!IsAttackProcess(true))
			{
				StartNextComboAttack(PlayerInput.NormalAttack, PlayerState.NormalAttack);
			}
			else // 공격 중이라면
			{
				if (nextCombo == PlayerInput.None)
				{
					SetNextCombo(PlayerInput.NormalAttack);
				}
			}
		}

		public void OnSpecialAttack(InputAction.CallbackContext context)
		{
			// 피격 중이거나, 스턴 상태면 리턴
			if(playerData.isStun || !hitCoolTimeIsEnd) { return; }

			// Idle, Move, Attack 관련 State가 아니면 리턴
			if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return; }

			if (context.started)
			{
				FDebug.Log("Special");
				if (!IsAttackProcess(true))
				{
					StartNextComboAttack(PlayerInput.SpecialAttack, curCombo != PlayerInput.NormalAttack ? PlayerState.ChargedAttack : PlayerState.NormalAttack);
				}
				else
				{
					if (nextCombo == PlayerInput.None && curCombo != PlayerInput.SpecialAttack)
					{
						SetNextCombo(PlayerInput.SpecialAttack);
					}
				}
			}
			else if (context.canceled && currentAttackState == PlayerState.ChargedAttack)
			{
				specialIsReleased = true;
			}
		}*/
	#endregion

	public AttackNode FindInput(PlayerInputEnum input)
	{
		AttackNode compareNode = curNode.childNodes.Count == 0 ? commandTree.Top : curNode;
		PlayerInputEnum nextNodeInput = input;

		if(commandTree.IsTopNode(compareNode)															// 하나의 콤보가 모두 끝난 상태이고,
			&& firstBehaiviorNode != null && firstBehaiviorNode.command != PlayerInputEnum.None // 콤보를 진행 중이며,
			&& firstBehaiviorNode.command != input)											// 해당 콤보가 입력값과 다르다면
		{
			nextNodeInput = firstBehaiviorNode.command;

			return null;
		}

		AttackNode node = commandTree.FindNode(nextNodeInput, compareNode);


		return node;
	}

	public void SetGauntlet(bool isEnabled)
	{
		glove.SetActive(isEnabled);
		rushGlove.SetActive(isEnabled);
	}

	public void SetCollider(bool isEnabled)
	{
		basicCollider.enabled = isEnabled;

		if(isEnabled)
		{
			attackColliderChanger.EnableCollider(curNode.attackColliderType);
			autoTargetColliderChanger.EnableCollider(curNode.attackColliderType);
		}
		else
		{
			attackColliderChanger.DisableAllCollider();
			autoTargetColliderChanger.DisableAllCollider();
		}
	}

	public bool IsAttackProcess(bool isContainedAfterDelay = false)
	{
		bool isAttack = IsCurrentState(PlayerState.AttackDelay) || (IsCurrentState(PlayerState.NormalAttack) || IsCurrentState(PlayerState.ChargedAttack));
		return (isContainedAfterDelay ? (isAttack || IsCurrentState(PlayerState.AttackAfterDelay)) : (isAttack));
	}

	private IEnumerator DashDelayCoroutine()
	{
		while(true)
		{
			if(!dashCoolTimeIsEnd)
			{
				yield return dashCoolTimeWFS;
				dashCoolTimeIsEnd = true;
				currentDashCount = maxDashCount;
			}
			yield return null;
		}
	}

	private IEnumerator HitDelayCoroutine()
	{
		while (true)
		{
			if (!hitCoolTimeIsEnd)
			{
				yield return hitCoolTimeWFS;
				hitCoolTimeIsEnd = true;
			}
			yield return null;
		}
	}

	public void ResetCombo()
	{
		nextCombo = PlayerInputEnum.None;
		curNode = unit.commandTree.Top;
		curCombo = PlayerInputEnum.None;
		currentAttackState = PlayerState.Idle;
		firstBehaiviorNode = null;
		LockNextCombo(false);

		comboGaugeSystem.ResetComboCount();
	}

	public bool NodeTransitionProc(PlayerInputEnum input, PlayerState nextAttackState)
	{
		if(comboIsLock) { return false; }
		
		AttackNode node = FindInput(input);

		if (node == null) 
		{
			LockNextCombo(true);
			return false; 
		}

		curNode = node;
		curCombo = node.command;
		currentAttackState = nextAttackState;
		currentAttackAnimKey = ComboAttackAnimaKey;

		if (commandTree.IsTopNode(curNode.parent))
		{
			firstBehaiviorNode = curNode;
		}

		return true;
	}

	public void SetNextCombo(PlayerInputEnum nextCommand)
	{
		if(!comboIsLock)
		{
			// 마지막 콤보에서 입력 씹는 코드
			if(curNode.childNodes.Count == 0 && !IsCurrentState(PlayerState.AttackAfterDelay)) { return; }

			nextCombo = nextCommand;
		}
	}

	public void LockNextCombo(bool isLock)
	{
		comboIsLock = isLock;

		if(comboIsLock) { nextCombo = PlayerInputEnum.None; }
	}
	
	public void StartNextComboAttack(PlayerInputEnum input, PlayerState nextAttackState)
	{
		if (nextCombo != PlayerInputEnum.None) input = nextCombo;
		if (!NodeTransitionProc(input, nextAttackState)) { return; }

		nextCombo = PlayerInputEnum.None;
		LockNextCombo(false);
		lastMoveDir = Vector3.zero;
		ChangeState(PlayerState.AttackDelay);
	}


	#region Move
	public void LerpToWorldPosition(Vector3 worldPos, float time)
	{
		UnitState<PlayerController> state = null;
		GetState(PlayerState.AutoMove, ref state);

		if(state == null) { FDebug.LogError("[PlayerController]AutoMoveSate Is Null"); }

		((PlayerAutoMoveState)state).SetAutoMove(worldPos, time);
		ChangeState(PlayerState.AutoMove);
	}
	public Vector3 RotatePlayer(Vector3 dir, bool isLerp = false)
	{
		Vector3 rotVec = Quaternion.AngleAxis(45, Vector3.up) * dir;

		if (rotVec == Vector3.zero) { return Vector3.zero; }

		if(isLerp)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotVec), rotatePower * Time.deltaTime);
		}
		else
		{
			transform.rotation = Quaternion.LookRotation(rotVec);
		}

		return rotVec;
	}
	#endregion

	#region Util
	private bool CheckReference(out List<string> msgs)
	{
		bool isClear = false;

		msgs = new List<string>();

		if (glove == null) { msgs.Add("glove is Null."); }
		if (rushGlove == null) { msgs.Add("rushGlove is Null."); }
		if (playerData == null) { msgs.Add("playerData is Null."); }
		if (commandTreeLoader == null) { msgs.Add("commandTreeLoader is Null."); }
		if (activePartController == null) { msgs.Add("activePartController is Null."); }
		if (comboGaugeSystem == null) { msgs.Add("comboGaugeSystem is Null."); }
		if (hitCountSystem == null) { msgs.Add("hitCountSystem is Null."); }
		if (attackColliderChanger == null) { msgs.Add("attackColliderChanger is Null."); }
		if (autoTargetColliderChanger == null) { msgs.Add("autoTargetColliderChanger is Null."); }
		if (basicCollider == null) { msgs.Add("basicCollider is Null."); }
		if (effectController == null) { msgs.Add("effectManager is Null."); }
		if (effectSO == null) { msgs.Add("effectSO is Null."); }
		//if (buffProvider == null) { msgs.Add("buffProvider is Null."); }
		if (rmController == null) { msgs.Add("rmController is Null."); }
		if (playerAnimationEvents == null) { msgs.Add("playerAnimationEvents is Null."); }
		if (animator == null) { msgs.Add("animator is Null."); }
		if (rigid == null) { msgs.Add("rigid is Null."); }
		if (rmController == null) { msgs.Add("rmController is Null."); }
		if(partSystem == null){msgs.Add("partSystem is Null");}

		isClear = msgs.Count == 0;
		if (isClear)
		{
			msgs.Add("Reference Check Clear");
		}

		return isClear;
	}
	#endregion
}