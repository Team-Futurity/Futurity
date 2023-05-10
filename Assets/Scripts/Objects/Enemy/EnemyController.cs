using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class EnemyController : UnitFSM<EnemyController>, IFSM
{
	public enum EnemyState : int
	{
		Idle,       //대기
		Default,
		MoveIdle,   //대기 중 랜덤 이동
		Chase,      //추격
		Attack,     //공격
		Hitted,     //피격
		Death,      //사망
	}

	//spawn
	private bool isSpawning;
	private float curSpawningTime;
	[SerializeField] private float maxSpawningTime = 2f;

	//Reference
	public UnitBase target;
	public Enemy enemyData;
	public Animator animator;
	public Rigidbody rigid;

	public CapsuleCollider chaseRange;
	public SphereCollider atkRange;
	public SphereCollider atkCollider;

	//Idle Properties
	public bool isChasing = false;
	public float stayCurTime = 0f;
	public float stayMaxTime = 3f;

	//Default Properties
	public float movePercentage = 5f;
	public float randMoveFloat;

	//MoveIdle Properties
	public GameObject transformParent;
	public GameObject moveIdleSpot;
	public float setTime = 2f;

	//Attack Properties
	public float attackAnimTime;
	public float attackCurTime;

	//Hitted Properties
	public bool isHitting = false;


	//FSM 버그인건지 뭔지 아무튼 임시
	public bool isDefault;
	public bool isMoveIdle;
	public bool isChase;
	public bool isAttack;
	public bool isHitted;
	public bool isDeath;



	private void Start()
	{
		//Basic Set Up
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		atkCollider.enabled = false;

		SetUp(EnemyState.Idle);
		unit = this;

		//spawning event
		isSpawning = true;
		curSpawningTime = 0f;
	}


	private void Update()
	{
		//spawning event
		if (isSpawning)
		{
			if (curSpawningTime <= maxSpawningTime)
			{
				curSpawningTime += Time.deltaTime;
				this.GetComponent<BoxCollider>().enabled = false;
			}
			else
			{
				if (!unit.IsCurrentState(EnemyState.Default))
				{
					unit.ChangeState(EnemyState.Default);
				}
				this.GetComponent<BoxCollider>().enabled = true;
				isSpawning = false;
			}
		}
		else
			curSpawningTime = 0f;

		//Hitted event


		//Death event
		if(enemyData.CurrentHp <= 0)
		{
			if (!IsCurrentState(EnemyState.Death))
			{
				ChangeState(EnemyState.Death);
			}
		}


		//이아래로 싹다 임시

		if (isDefault)
		{
			stayCurTime += Time.deltaTime;

			if (stayCurTime > stayMaxTime)
			{
				if (randMoveFloat < unit.movePercentage)
				{
					if (!IsCurrentState(EnemyState.MoveIdle))
					{
						ChangeState(EnemyState.MoveIdle);
					}
				}
				else
				{
					if (!IsCurrentState(EnemyState.Idle))
					{
						ChangeState(EnemyState.Idle);
					}
				}
			}
		}

		else if (isMoveIdle)
		{
			unit.transform.rotation = Quaternion.Lerp(unit.transform.rotation, Quaternion.LookRotation(unit.moveIdleSpot.transform.position), 30.0f * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position,
				moveIdleSpot.transform.position,
				enemyData.Speed * Time.deltaTime);

			if (transform.position == moveIdleSpot.transform.position)
			{
				rigid.velocity = Vector3.zero;
				if (!IsCurrentState(EnemyState.Default))
				{
					ChangeState(EnemyState.Default);
				}
			}
		}

		else if (isChase)
		{
			if (target == null)
				return;
			//transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position), 30.0f * Time.deltaTime);
			transform.LookAt(target.transform.position);
			float distance = Vector3.Distance(transform.position, target.transform.position);
			transform.position += transform.forward * enemyData.Speed * Time.deltaTime;
		}

		else if(isAttack)
		{
			attackCurTime += Time.deltaTime;

			if (attackCurTime > attackAnimTime)
			{
				if (!IsCurrentState(EnemyState.Chase))
				{
					ChangeState(EnemyState.Chase);
				}
			}
		}
	}
}
