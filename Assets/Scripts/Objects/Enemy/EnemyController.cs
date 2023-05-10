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


	protected override void Update()
	{
		base.Update();
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
	}
}
