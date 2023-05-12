using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.XR;


public class PlayerController : UnitFSM<PlayerController>, IFSM
{
	public enum PlayerState : int
	{
		Idle,           // ���
		Attack,         // ����
			NormalAttack,	// �Ϲݰ��� 
			ChargedAttack,	// ��������
		AttackDelay,    // ���� �� ������
		Hit,            // �ǰ�
		Move,           // �̵�
		Dash,           // ���
		Stun,           // ����
		Death,          // ���
	}

	public enum PlayerInput : int
	{
		None,
		NormalAttack,
		SpecialAttack,
		Dash
	}

	// reference
	public Player playerData;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public TrailRenderer dashEffect;

	// move
	//public Vector3 moveInput;
	public Vector3 moveDir;

	// attack
	public PlayerInput curCombo;
	public AttackNode curNode;
	public Tree comboTree;
	public RadiusCapsuleCollider attackCollider;

	// input
	public bool specialIsReleased = false;

	//�ӽ�
	public GameObject glove;

	// sound 
	public FMODUnity.EventReference dash;
	public FMODUnity.EventReference hitMelee;
	public FMODUnity.EventReference hitRanged;

	// coroutine 
	public bool isRush;

	// math
	private readonly int Meter = 100; // centimeter ����

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		dashEffect = GetComponent<TrailRenderer>();

		SetUp(PlayerState.Idle);
		unit = this;
		curNode = comboTree.top;
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		//if (IsCurrentState(PlayerState.Hit) || IsCurrentState(PlayerState.Stun))
		//	return;

		Vector3 input = context.ReadValue<Vector3>();
		if (input != null)
		{
			moveDir = new Vector3(input.x, 0f, input.y);

			if (!IsCurrentState(PlayerState.Move))
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
		if (context.started)
		{
			AttackNode node = FindInput(PlayerInput.NormalAttack);
			FDebug.Log("In");
			if (node != null && !IsCurrentState(PlayerState.Attack))
			{
				curNode = node;
				curCombo = node.command;
				ChangeState(PlayerState.Attack);
			}
		}
	}

	public void OnSpecialAttack(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			// ���� ��尡 top������� üũ
			// top����� ��, �޺� �Է� ���� �ƴ϶�� ��.
			bool isInit = curNode == comboTree.top;

			AttackNode node = FindInput(PlayerInput.SpecialAttack);
			if (node != null )
			{
				curNode = node;
				curCombo = node.command;

				if (isInit) // �޺� �Է� ���� �ƴϸ� ����
				{
					ChangeState(PlayerState.ChargedAttack);
				}
				else        // �޺� �Է� ���̸� �Ϲ�
				{
					ChangeState(PlayerState.NormalAttack);
				}
			}
		}
		else if(context.canceled)
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

	float testCurTime = 0;

	public IEnumerator ChargedAttackProc(float attackST, float attackLengthMark)
	{
		while (true)
		{
			if (isRush)
			{
				Vector3 targetPos = transform.position + transform.forward * (attackLengthMark / Meter);
				testCurTime = 0;
				while (transform.position.magnitude < targetPos.magnitude && isRush)
				{
					// �� ������ �� �̵� �ӵ� ���(m/Frame)
					float moveSpeed = (attackLengthMark * Time.deltaTime) / (Meter * curNode.attackDelay);

					rigid.velocity = transform.forward * moveSpeed;

					//transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);

					testCurTime += Time.deltaTime;

					yield return null;
				}
				FDebug.Log(testCurTime);

				transform.position = targetPos;
				isRush = false;
				ChangeState(PlayerState.AttackDelay);
			}
			yield return null;
		}
	}
}