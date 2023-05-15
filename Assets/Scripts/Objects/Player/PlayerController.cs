using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : UnitFSM<PlayerController>, IFSM
{
	public enum PlayerState : int
	{
		Idle,           // ���

		// ����
		AttackDelay,		// ���� �� ������
		NormalAttack,		// �Ϲݰ��� 
		ChargedAttack,		// ��������
		AttackAfterDelay,	// ���� �� ������

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
	public readonly string ComboAttackAnimaKey = "Combo";
	public readonly string ChargedAttackAnimaKey = "Charging";

	// reference
	public Player playerData;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public TrailRenderer dashEffect;
	[HideInInspector] public CapsuleCollider basicCollider;

	// move
	//public Vector3 moveInput;
	public Vector3 moveDir;

	// attack
	public PlayerInput curCombo;
	public PlayerInput nextCombo;
	public AttackNode curNode;
	public Tree comboTree;
	public RadiusCapsuleCollider attackCollider;
	public PlayerState currentAttackState;

	// input
	public bool specialIsReleased = false;

	//�ӽ�
	public GameObject glove;
	public struct EffectData
	{
		public Transform effectPos;
		public GameObject effect;
	}
	public List<EffectData> rushEffects;
	public ObjectPoolManager<Transform> rushObjectPool;

	// sound 
	public FMODUnity.EventReference dash;
	public FMODUnity.EventReference hitMelee;
	public FMODUnity.EventReference hitRanged;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		dashEffect = GetComponent<TrailRenderer>();
		basicCollider = GetComponent<CapsuleCollider>();

		unit = this;
		SetUp(PlayerState.Idle);
		curNode = comboTree.top;

		nextCombo = PlayerInput.None;

		glove.SetActive(false);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (IsCurrentState(PlayerState.Hit) || playerData.isStun || IsAttackProcess())
		{
			return;
		}
		

		Vector3 input = context.ReadValue<Vector3>();
		if (input != null)
		{
			moveDir = new Vector3(input.x, 0f, input.y);

			if(IsCurrentState(PlayerState.ChargedAttack))
			{
				AddSubState(PlayerState.Move);
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
				//ChangeState(PlayerState.NormalAttack);
				currentAttackState = PlayerState.NormalAttack;
				ChangeState(PlayerState.AttackDelay);
			}
			else
			{
				if(nextCombo == PlayerInput.None)
				{
					nextCombo = PlayerInput.NormalAttack;
				}
			}
		}
	}

	public void OnSpecialAttack(InputAction.CallbackContext context)
	{
		if (context.started && !playerData.isStun)
		{
			// ���� ��尡 top������� üũ
			// top����� ��, �޺� �Է� ���� �ƴ϶�� ��.
			//bool isInit = curNode == comboTree.top;

			if(!IsAttackProcess())
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
		else if (context.canceled)
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
	}

	public bool IsAttackProcess()
	{
  		 return IsCurrentState(PlayerState.AttackDelay) || (IsCurrentState(PlayerState.NormalAttack) || IsCurrentState(PlayerState.ChargedAttack));
	}
}