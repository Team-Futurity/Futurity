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
	public const float cm2m = 0.01f; // centimeter To meter
	public const float m2cm = 100f; // meter To centimeter

	[Header("[수치 조절]────────────────────────────────────────────────────────────────────────────────────────────")]

	// attack
	[Space(2)]
	[Header("콤보")]
	public CommandTree comboTree;

	// move
	[Space(5)]
	[Header("이동")]
	[Tooltip("회전하는 속도")]
	public float rotatePower;

	// dash
	[Space(5)]
	[Header("대시. 런타임 변경 불가")]
	public float dashCoolTime;
	public GameObject dashEffect;
	public Transform dashPos;
	public ObjectPoolManager<Transform> dashPoolManager;

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

	// dash
	[Space(5)]
	[Header("대시 관련")]
	public bool dashCoolTimeIsEnd = false;
	public bool comboIsEnd = false;

	// input
	[Space(5)]
	[Header("입력 관련")]
	public bool specialIsReleased = false;
	public bool moveIsPressed = false;
	private bool comboIsLock = false;

	// attack
	[Space(5)]
	[Header("공격 관련")]
	public PlayerInput curCombo;
	public PlayerInput nextCombo;
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
	public RadiusCapsuleCollider attackCollider;
	public RadiusCapsuleCollider autoTargetCollider;
	public CapsuleCollider basicCollider;
	public EffectController effectManager;
	public EffectDatas effectSO;
	public BuffProvider buffProvider;
	public RootMotionContoller rmController;
	public PlayerAnimationEvents playerAnimationEvents;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
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
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();

		// effect
		effectManager = ECManager.Instance.GetEffectManager(effectSO);

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

		// UnitFSM Init
		SetFSM();
		UnitState<PlayerController> astate = null;
		GetState(PlayerState.AttackAfterDelay, ref astate);
		nextStateEvent.AddListener((state) => { ((PlayerAttackAfterDelayState)astate).NextAttackState(unit, state); });

		// Attack Init
		//comboTree = commandTreeLoader.GetCommandTree();
		curNode = comboTree.top;
		nextCombo = PlayerInput.None;
		firstBehaiviorNode = null;
		//comboTree.SetTree(comboTree.top, null);

		// Glove Init
		glove.SetActive(false);

		// dash
		dashPoolManager = new ObjectPoolManager<Transform>(dashEffect, gameObject);
		dashCoolTimeWFS = new WaitForSeconds(dashCoolTime);
		StartCoroutine(DashDelayCoroutine());

		// hit
		hitCoolTimeWFS = new WaitForSeconds(hitCoolTime);
		StartCoroutine(HitDelayCoroutine());

		
	}

	public void SetFSM()
	{
		unit = this;
		SetUp(PlayerState.Idle);
	}

	#region Input
	public string GetInputData(PlayerInput input, bool isProcess, params string[] additionalDatas)
	{
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

		return returnValue;
	}

	public string MoveProcess(InputAction.CallbackContext context)
	{
		Vector3 input = context.ReadValue<Vector3>();

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
					return GetInputData(PlayerInput.Move, true, input.ToString(), "SubState");
				}
			}
			return GetInputData(PlayerInput.Move, false, input.ToString());
		}

		if (playerData.isStun || !hitCoolTimeIsEnd)
		{
			return GetInputData(PlayerInput.Move, false, input.ToString()); ;
		}

		// 이동 기능
		if (!IsCurrentState(PlayerState.Move))
		{
			ChangeState(PlayerState.Move);
			return GetInputData(PlayerInput.Move, true, input.ToString(), "State");
		}

		return GetInputData(PlayerInput.Move, false, input.ToString());
	}

	public string DashProcess(InputAction.CallbackContext context)
	{
		if (IsCurrentState(PlayerState.Hit) || playerData.isStun || !dashCoolTimeIsEnd || !hitCoolTimeIsEnd) 
		{ 
			return GetInputData(PlayerInput.Dash, false); 
		}

		if (!IsCurrentState(PlayerState.Dash))
		{
			ChangeState(PlayerState.Dash);
			return GetInputData(PlayerInput.Dash, true);
		}

		return GetInputData(PlayerInput.Dash, false);
	}

	// Normal Attack
	public string NAProcess(InputAction.CallbackContext context)
	{
		// 피격 중이거나, 스턴 상태면 리턴
		if (playerData.isStun || !hitCoolTimeIsEnd) { return GetInputData(PlayerInput.NormalAttack, false); }

		// Idle, Move, Attack 관련 State가 아니면 리턴
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return GetInputData(PlayerInput.NormalAttack, false); }

		// AfterDelay나 다른 스테이트(Idle, Move)라면
		if (!IsAttackProcess(true))
		{
			StartNextComboAttack(PlayerInput.NormalAttack, PlayerState.NormalAttack);

			return GetInputData(PlayerInput.NormalAttack, true, currentAttackState.ToString(), curNode.name);
		}
		else // 공격 중이라면
		{
			if (nextCombo == PlayerInput.None)
			{
				SetNextCombo(PlayerInput.NormalAttack);
				return GetInputData(PlayerInput.NormalAttack, true, "Queueing", FindInput(PlayerInput.NormalAttack).name);
			}
		}
		
		return GetInputData(PlayerInput.NormalAttack, false);
	}

	// Special Attack
	public string SAProcess(InputAction.CallbackContext context)
	{
		// 피격 중이거나, 스턴 상태면 리턴
		if (playerData.isStun || !hitCoolTimeIsEnd) { return GetInputData(PlayerInput.SpecialAttack, false); }

		// Idle, Move, Attack 관련 State가 아니면 리턴
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return GetInputData(PlayerInput.SpecialAttack, false); }

		var state = curCombo != PlayerInput.NormalAttack ? PlayerState.ChargedAttack : PlayerState.NormalAttack;
		if (context.started)
		{
			if (!IsAttackProcess(true))
			{
				StartNextComboAttack(PlayerInput.SpecialAttack, state);
				return GetInputData(PlayerInput.SpecialAttack, true, state.ToString(), state == PlayerState.NormalAttack ? curNode.name : "Pressed");
			}
			else
			{
				if (nextCombo == PlayerInput.None && curCombo != PlayerInput.SpecialAttack)
				{
					SetNextCombo(PlayerInput.SpecialAttack);
					return GetInputData(PlayerInput.SpecialAttack, true, "Queueing");
				}
			}
		}
		else
		{
			if (context.canceled && currentAttackState == PlayerState.ChargedAttack)
			{
				specialIsReleased = true;
				return GetInputData(PlayerInput.SpecialAttack, true, state.ToString(), "Released");
			}
		}
		
		return GetInputData(PlayerInput.SpecialAttack, false);
	}

	// Special Move
	public string SMProcess(InputAction.CallbackContext context)
	{
		if (!activePartIsActive) { return GetInputData(PlayerInput.SpecialMove, false); }

		activePartController.RunActivePart(this, playerData, SpecialMoveType.Basic);
		return GetInputData(PlayerInput.SpecialAttack, true, SpecialMoveType.Basic.ToString());
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

	public AttackNode FindInput(PlayerInput input)
	{
		AttackNode compareNode = curNode.childNodes.Count == 0 ? comboTree.top : curNode;
		PlayerInput nextNodeInput = input;

		if(IsTopNode(compareNode)															// 하나의 콤보가 모두 끝난 상태이고,
			&& firstBehaiviorNode != null && firstBehaiviorNode.command != PlayerInput.None // 콤보를 진행 중이며,
			&& firstBehaiviorNode.command != input)											// 해당 콤보가 입력값과 다르다면
		{
			nextNodeInput = firstBehaiviorNode.command;

			return null;
		}

		AttackNode node = comboTree.FindNode(nextNodeInput, compareNode);


		return node;
	}

	public void SetCollider(bool isEnabled)
	{
		basicCollider.enabled = isEnabled;
		attackCollider.enabled = isEnabled;
		autoTargetCollider.enabled = isEnabled;
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
		nextCombo = PlayerInput.None;
		curNode = unit.comboTree.top;
		curCombo = PlayerInput.None;
		currentAttackState = PlayerState.Idle;
		firstBehaiviorNode = null;
		LockNextCombo(false);

		comboGaugeSystem.ResetComboCount();
	}

	public bool IsTopNode(AttackNode node) => node == comboTree.top;

	public bool NodeTransitionProc(PlayerInput input, PlayerState nextAttackState)
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

		if (IsTopNode(curNode.parent))
		{
			firstBehaiviorNode = curNode;
		}

		return true;
	}

	public void SetNextCombo(PlayerInput nextCommand)
	{
		if(!comboIsLock)
		{
			nextCombo = nextCommand;
		}
	}

	public void LockNextCombo(bool isLock)
	{
		comboIsLock = isLock;

		if(comboIsLock) { nextCombo = PlayerInput.None; }
	}
	
	public void StartNextComboAttack(PlayerInput input, PlayerState nextAttackState)
	{
		if (nextCombo != PlayerInput.None) input = nextCombo;
		if (!NodeTransitionProc(input, nextAttackState)) { return; }

		nextCombo = PlayerInput.None;
		LockNextCombo(false);
		ChangeState(PlayerState.AttackDelay);
	}

	public void LerpToWorldPosition(Vector3 worldPos, float time)
	{
		UnitState<PlayerController> state = null;
		GetState(PlayerState.AutoMove, ref state);

		if(state == null) { FDebug.LogError("[PlayerController]AutoMoveSate Is Null"); }

		((PlayerAutoMoveState)state).SetAutoMove(worldPos, time);
		ChangeState(PlayerState.AutoMove);
	}

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
		if (attackCollider == null) { msgs.Add("attackCollider is Null."); }
		if (autoTargetCollider == null) { msgs.Add("autoTargetCollider is Null."); }
		if (basicCollider == null) { msgs.Add("basicCollider is Null."); }
		if (effectManager == null) { msgs.Add("effectManager is Null."); }
		if (effectSO == null) { msgs.Add("effectSO is Null."); }
		if (buffProvider == null) { msgs.Add("buffProvider is Null."); }
		if (rmController == null) { msgs.Add("rmController is Null."); }
		if (playerAnimationEvents == null) { msgs.Add("playerAnimationEvents is Null."); }
		if (animator == null) { msgs.Add("animator is Null."); }
		if (rigid == null) { msgs.Add("rigid is Null."); }
		if (rmController == null) { msgs.Add("rmController is Null."); }

		isClear = msgs.Count == 0;
		if (isClear)
		{
			msgs.Add("Reference Check Clear");
		}

		return isClear;
	}
	#endregion
}