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
	public RadiusCapsuleCollider attackCollider;
	public RadiusCapsuleCollider autoTargetCollider;
	[HideInInspector] public CapsuleCollider basicCollider;
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
		basicCollider = GetComponent<CapsuleCollider>();

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
		if (IsCurrentState(PlayerState.Hit) || playerData.isStun)
		{
			return;
		}
		

		Vector3 input = context.ReadValue<Vector3>();
		if (input != null)
		{
			moveDir = new Vector3(input.x, 0f, input.y);

			if (IsAttackProcess())
			{
				if (IsCurrentState(PlayerState.ChargedAttack))
				{
					//AddSubState(PlayerState.Move);
				}
				return;

			}
			else if (!IsCurrentState(PlayerState.Move))
			{
				ChangeState(PlayerState.Move);
			}
		}
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		/*if (IsCurrentState(PlayerState.Hit) || IsCurrentState(PlayerState.Stun))
			return;
*/
		if (context.performed && !playerData.isStun)
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
		if (context.started && !playerData.isStun)
		{
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
			else
			{
				if (nextCombo == PlayerInput.None)
				{
					nextCombo = PlayerInput.NormalAttack;
				}
			}
		}
	}

	public void OnSpecialAttack(InputAction.CallbackContext context)
	{
		if(playerData.isStun) { return; }

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

	public bool IsAttackProcess()
	{
		return IsCurrentState(PlayerState.AttackDelay) || (IsCurrentState(PlayerState.NormalAttack) || IsCurrentState(PlayerState.ChargedAttack));
	}
}