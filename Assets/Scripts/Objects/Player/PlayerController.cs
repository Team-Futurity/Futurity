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
		Idle,           // 대기
		Attack,         // 공격
			NormalAttack,	// 일반공격 
			ChargedAttack,	// 차지공격
		AttackDelay,    // 공격 후 딜레이
		Hit,            // 피격
		Move,           // 이동
		Dash,           // 대시
		Stun,           // 기절
		Death,          // 사망
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
	[HideInInspector] public CapsuleCollider basicCollider;

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

	//임시
	public GameObject glove;

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
			// 현재 노드가 top노드인지 체크
			// top노드라는 건, 콤보 입력 중이 아니라는 것.
			bool isInit = curNode == comboTree.top;

			AttackNode node = FindInput(PlayerInput.SpecialAttack);
			if (node != null )
			{
				curNode = node;
				curCombo = node.command;

				if (isInit) // 콤보 입력 중이 아니면 차지
				{
					ChangeState(PlayerState.ChargedAttack);
				}
				else        // 콤보 입력 중이면 일반
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

	/*public IEnumerator ChargedAttackProc(float attackST, float attackLengthMark, float moveSpeed)
	{
		WaitForSeconds fixedDuration = new WaitForSeconds(Time.fixedDeltaTime);

		while (true)
		{
			if (isRush)
			{
				// 필요 변수 세팅
				Vector3 forward = transform.forward;										// 시선 벡터
				Vector3 originPos = transform.position;										// 돌진 전 위치
				Vector3 targetPos = originPos + forward * (attackLengthMark / Meter);		// 목표 위치
				float targetMagnitude = (targetPos - originPos).magnitude;					// originPos에서 targetPos로 향하는 벡터의 크기^2
				float rayLength = moveSpeed * Time.fixedDeltaTime + basicCollider.radius;	// ray의 길이
				Ray ray = new Ray(originPos, forward);										// 사용할 ray
				RaycastHit hit;																// RayCastHit
				
				// 돌진 전 위치에서 현재 위치로 향하는 벡터의 크기가 targetMagnitude보다 작고
				// isRush가 참인 동안
				// 해당 코드 반복
				while (((transform.position - originPos).magnitude < targetMagnitude) && isRush)
				{
					// 디버깅용 Ray
					FDebug.DrawRay(transform.position, forward * rayLength, UnityEngine.Color.red);

					// Collision연산으로 부족한 부분을 메꿀 Ray연산
					// ray의 길이는 조금 논의가 필요할지도...?
					if (Physics.Raycast(ray, out hit, rayLength, layer))
					{
						isRush = false;
						break;
					}

					// while문이 도는 동안 속도를 moveSpeed로 고정
					rigid.velocity = forward * moveSpeed;

					// 물리 연산이므로 FixedUpdate에서 처리하 듯 
					yield return fixedDuration;
				}

				if(isRush)
				{
					transform.position = targetPos;
					isRush = false;
				}

				ChangeState(PlayerState.AttackDelay);
			}
			yield return null;
		}
	}*/
}