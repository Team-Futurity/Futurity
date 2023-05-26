using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerController : UnitFSM<PlayerController>, IFSM
{
	public enum PlayerState : int
	{
		Idle,           // ���

		// ����
		AttackDelay,        // ���� �� ������
		NormalAttack,       // �Ϲݰ��� 
		ChargedAttack,      // ��������
		AttackAfterDelay,   // ���� �� ������

		Hit,            // �ǰ�
		Move,           // �̵�
		Dash,           // ���
		Death,          // ���
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

	[Header("[��ġ ����]����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������")]

	// attack
	[Space(2)]
	[Header("�޺�")]
	public Tree comboTree;

	// dash
	[Space(5)]
	[Header("���. ��Ÿ�� ���� �Ұ�")]
	public float dashCoolTime;

	[Space(15)]
	[Header("[������]������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������")]

	// move
	[Space(2)]
	[Header("�̵� ����")]
	public Vector3 moveDir;

	// dash
	[Space(5)]
	[Header("��� ����")]
	public bool coolTimeIsEnd = false;

	// input
	[Space(5)]
	[Header("�Է� ����")]
	public bool specialIsReleased = false;
	public bool moveIsPressed = false;

	// attack
	[Space(5)]
	[Header("���� ����")]
	public PlayerInput curCombo;
	public PlayerInput nextCombo;
	public AttackNode curNode;
	public PlayerState currentAttackState;
	[HideInInspector] public string currentAttackAnimKey;

	[Space(15)]
	[Header("[���� 1ȸ �Ҵ�]������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������")]

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
	private WaitForSeconds dashCoolTimeWFS;

	// event
	[HideInInspector] public UnityEvent<PlayerState> nextStateEvent;
	[HideInInspector] public InputAction moveAction;

	// Temporary
	[Serializable]
	public struct EffectData
	{
		public Transform effectPos;
		public GameObject effect;
	}
	[Space(5)]
	[Header("���� ����Ʈ")]
	public List<EffectData> rushEffects;
	public ObjectPoolManager<Transform> rushObjectPool;
	public ObjectPoolManager<Transform> rushObjectPool2;

	// sound 
	[Space(5)]
	[Header("����")]
	public FMODUnity.EventReference dash;
	public FMODUnity.EventReference hitMelee;
	public FMODUnity.EventReference hitRanged;

	// etc
	

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
		UnitState<PlayerController> astate = null;
		GetState(PlayerState.AttackAfterDelay, ref astate);
		nextStateEvent.AddListener((state) => { ((PlayerAttackAfterDelayState)astate).NextAttackState(unit, state); });

		// Attack Init
		curNode = comboTree.top;
		nextCombo = PlayerInput.None;

		// Glove Init
		glove.SetActive(false);

		// dash
		dashCoolTimeWFS = new WaitForSeconds(dashCoolTime);
		StartCoroutine(DashDelayCoroutine());
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		// Input
		Vector3 input = context.ReadValue<Vector3>();
		if (input == null) { return; }
		moveDir = new Vector3(input.x, 0f, input.y);
		moveAction = context.action;

		moveIsPressed = (!context.started || context.performed) ^ context.canceled && moveDir != Vector3.zero;

		// ����ó��
		if (!IsCurrentState(PlayerState.Idle))
		{
			// ���� �� �̵� ���
			if (IsAttackProcess())
			{
				if (IsCurrentState(PlayerState.ChargedAttack))
				{
					//AddSubState(PlayerState.Move);
				}
			}
			return;
		}

		// �̵� ���
		if (!IsCurrentState(PlayerState.Move))
		{
			ChangeState(PlayerState.Move);
		}
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if (IsCurrentState(PlayerState.Hit) || playerData.isStun || !coolTimeIsEnd) { return; }

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
		// �Է��� ���� �ʾ�����(Pressed ������ �ƴϸ�) ����
		if (!context.started) { return; }

		// Idle, Move, Attack ���� State�� �ƴϸ� ����
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return; }

		// AfterDelay�� �ٸ� ������Ʈ(Idle, Move)���
		if (!IsAttackProcess())
		{
			AttackNode node = FindInput(PlayerInput.NormalAttack);

			if (node == null) { return; }

			curNode = node;
			curCombo = node.command;
			currentAttackState = PlayerState.NormalAttack;
			currentAttackAnimKey = ComboAttackAnimaKey;

			nextStateEvent.Invoke(PlayerState.AttackDelay);
		}
		else // ���� ���̶��
		{
			if (nextCombo == PlayerInput.None)
			{
				nextCombo = PlayerInput.NormalAttack;
			}
		}
	}

	public void OnSpecialAttack(InputAction.CallbackContext context)
	{
		// Idle, Move, Attack ���� State�� �ƴϸ� ����
		if (!IsCurrentState(PlayerState.Move) && !IsCurrentState(PlayerState.Idle) && !IsAttackProcess(true)) { return; }

		if (context.started)
		{
			if (!IsAttackProcess())
			{
				AttackNode node = FindInput(PlayerInput.SpecialAttack);
				if (node == null) { return; }

				curNode = node;
				curCombo = node.command;

				if (!IsCurrentState(PlayerState.AttackAfterDelay)) // �޺� �Է� ���� �ƴϸ� ����
				{
					currentAttackState = PlayerState.ChargedAttack;
					//ChangeState(PlayerState.ChargedAttack);
				}
				else // �޺� �Է� ���̸� �Ϲ�
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

	private IEnumerator DashDelayCoroutine()
	{
		while(true)
		{
			if(!coolTimeIsEnd)
			{
				yield return dashCoolTimeWFS;
				coolTimeIsEnd = true;
			}
			yield return null;
		}
	}
}