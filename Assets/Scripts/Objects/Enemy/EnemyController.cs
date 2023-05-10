using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
	[HideInInspector] public UnitBase target;
	[HideInInspector] public Enemy enemyData;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public Material eMaterial;

	public CapsuleCollider chaseRange;
	public SphereCollider atkRange;
	public SphereCollider atkCollider;

	//Idle Properties
	[HideInInspector] public bool isChasing = false;
	[HideInInspector] public float stayCurTime = 0f;
	public float staySetTime = 3f;

	//Default Properties
	public float movePercentage = 5f;
	[HideInInspector] public float randMoveFloat;

	//MoveIdle Properties
	public GameObject transformParent;
	[HideInInspector] public GameObject moveIdleSpot;
	public float idleSetTime = 2f;

	//Attack Properties
	public float attackSetTime = 2f;
	[HideInInspector] public float attackCurTime;

	//Hitted Properties
	public float hitMaxTime = 1f;
	[HideInInspector] public float hitCurTime;
	[HideInInspector] public Color defaultColor = new Color(55f, 55f, 55f, 255f);

	//animation name
	public readonly string moveAnimParam = "Move";
	public readonly string atkAnimParam = "Attack";
	public readonly string hitAnimParam = "Hit";



	private void Start()
	{
		//Basic Set Up
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		eMaterial = GetComponentInChildren<SkinnedMeshRenderer>().material;
		atkCollider.enabled = false;

		unit = this;
		SetUp(EnemyState.Idle);

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
