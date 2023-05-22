using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : UnitFSM<PlayerController>, IFSM
{
	public enum PlayerState : int
	{
		Idle,           // 대기

		// 공격
		AttackDelay,        // 공격 전 딜레이
		NormalAttack,       // 일반공격 
		ChargedAttack,      // 차지공격
		AttackAfterDelay,   // 공격 후 딜레이

		Hit,            // 피격
		Move,           // 이동
		Dash,           // 대시
		Death,          // 사망
	}

	public enum PlayerInput : int
	{
		None,
		NormalAttack,
		SpecialAttack,
		Dash
	}

	// Constants
	public readonly string EnemyTag = "Enemy";
	public readonly string ComboAttackAnimaKey = "ComboParam";
	public readonly string ChargedAttackAnimaKey = "ChargingParam";
	public readonly string IsAttackingAnimKey = "IsAttacking";
	public const int NullState = -1;

	[Header("[수치 조절]────────────────────────────────────────────────────────────────────────────────────────────")]

	// attack
	[Space(2f)]
	[Header("콤보")]
	public Tree comboTree;

	[Space(15)]
	[Header("[디버깅용]─────────────────────────────────────────────────────────────────────────────────────────────")]

	// move
	[Space(2)]
	[Header("이동 관련")]
	public Vector3 moveDir;

	// input
	[Space(5)]
	[Header("입력 관련")]
	public bool specialIsReleased = false;

	// attack
	[Space(5)]
	[Header("공격 관련")]
	public PlayerInput curCombo;
	public PlayerInput nextCombo;
	public AttackNode curNode;
	public PlayerState currentAttackState;
	[HideInInspector] public string currentAttackAnimKey;

	[Space(15)]
	[Header("[최초 1회 할당]──────────────────────────────────────────────────────────────────────────────────────────")]

	// reference
	[Space(2)]
	[Header("References")]
	public GameObject glove;
	public Player playerData;
	public ComboGaugeSystem comboGaugeSystem;
	public RadiusCapsuleCollider attackCollider;
	public RadiusCapsuleCollider autoTargetCollider;
	public CapsuleCollider basicCollider;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public TrailRenderer dashEffect;

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

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		dashEffect = GetComponent<TrailRenderer>();

		// Animator Init
		animator.SetInteger(ComboAttackAnimaKey, NullState);
		animator.SetInteger(ChargedAttackAnimaKey, NullState);

		// UnitFSM Init
		unit = this;
		SetUp(PlayerState.Idle);

		// Attack Init
		curNode = comboTree.top;
		nextCombo = PlayerInput.None;

		// Glove Init
		glove.SetActive(false);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		// Input
		Vector3 input = context.ReadValue<Vector3>();
		if (input == null) { return; }
		moveDir = new Vector3(input.x, 0f, input.y);

		// 예외처리
		if (!IsCurrentState(PlayerState.Idle))
		{
			// 돌진 중 이동 기능
			if (IsAttackProcess())
			{
				if (IsCurrentState(PlayerState.ChargedAttack))
				{
					//AddSubState(PlayerState.Move);
				}
			}
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
		if (IsCurrentState(PlayerState.Hit) || playerData.isStun) { return; }

		if (context.performed)
		{
			if (!IsCurrentState(PlayerState.Dash))
			{
				curNode = comboTree.top;
				ChangeState(PlayerState.Dash);
			}
		}
	}

	public void OnNormalAttack(InputAction.CallbackContext context)
	{
		// 입력이 되지 않았으면(Pressed 시점이 아니면) 리턴
		if (!context.started) { return; }

		// Idle, Move, Attack 관련 State가 아니면 리턴
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return; }

		// AfterDelay나 다른 스테이트(Idle, Move)라면
		if (!IsAttackProcess())
		{
			AttackNode node = FindInput(PlayerInput.NormalAttack);

			if (node == null) { return; }

			curNode = node;
			curCombo = node.command;
			currentAttackState = PlayerState.NormalAttack;
			currentAttackAnimKey = ComboAttackAnimaKey;
			ChangeState(PlayerState.AttackDelay);
		}
		else // 공격 중이라면
		{
			if (nextCombo == PlayerInput.None)
			{
				nextCombo = PlayerInput.NormalAttack;
			}
		}
	}

	public void OnSpecialAttack(InputAction.CallbackContext context)
	{
		// Idle, Move, Attack 관련 State가 아니면 리턴
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return; }

		if (context.started)
		{
			if (!IsAttackProcess())
			{
				AttackNode node = FindInput(PlayerInput.SpecialAttack);
				if (node == null) { return; }

				curNode = node;
				curCombo = node.command;

				if (!IsCurrentState(PlayerState.AttackAfterDelay)) // 콤보 입력 중이 아니면 차지
				{
					currentAttackState = PlayerState.ChargedAttack;
					//ChangeState(PlayerState.ChargedAttack);
				}
				else // 콤보 입력 중이면 일반
				{
					currentAttackState = PlayerState.NormalAttack;
					//ChangeState(PlayerState.NormalAttack);
				}

				ChangeState(PlayerState.AttackDelay);
			}
			else
			{
				if (nextCombo == PlayerInput.None)
				{
					nextCombo = PlayerInput.SpecialAttack;
				}
			}
		}
		else if (context.canceled && currentAttackState != PlayerState.NormalAttack)
		{
			specialIsReleased = true;
		}
	}

	public AttackNode FindInput(PlayerInput input)
	{
		AttackNode node = comboTree.FindNode(input, curNode);

		if (node == null)
		{
			node = comboTree.FindNode(input, comboTree.top);
		}

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
}