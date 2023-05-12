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
		Idle,			//대기
		Default,
		MoveIdle,		//대기 중 랜덤 이동
		Hitted,			//피격
		Death,          //사망


		//Melee Default
		MDefaultChase,          //추격
		MDefaultAttack,         //공격

		//Ranged Default
		RDefaultChase,	
		RDefaultAttack,
	}

	public enum EnemyType : int
	{
		MeleeDefault,
		RangedDefault,

	}

	[SerializeField] private EnemyType enemyType;

	//spawn
	private bool isSpawning;
	private float curSpawningTime;
	[SerializeField] private float maxSpawningTime = 2f;
	private BoxCollider enemyCollider;

	//Reference
	/*[HideInInspector]*/ public UnitBase target;
	public Enemy enemyData;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	public Material eMaterial;

	public CapsuleCollider chaseRange;
	public SphereCollider atkRange;
	public SphereCollider atkCollider;

	//Idle Properties
	[HideInInspector] public bool isChasing = false;
	public float idleSetTime = 3f;

	//Default Properties
	public float movePercentage = 5f;
	[HideInInspector] public float randMoveFloat;

	//MoveIdle Properties
	public GameObject transformParent;
	[HideInInspector] public GameObject moveIdleSpot;

	//Attack Properties
	public float attackSetTime = 2f;
	public float distance;

	//Chase Properties
	public GameObject RangedBackPos;

	//Hitted Properties
	public float hitMaxTime = 1f;
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
		enemyCollider = GetComponent<BoxCollider>();
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
				enemyCollider.enabled = false;
			}
			else
			{
				unit.ChangeState(EnemyState.Default);
				enemyCollider.enabled = true;
				isSpawning = false;
			}
		}
		else
			curSpawningTime = 0f;
	}

	public void DelayChangeState (float curTime, float maxTime, EnemyController unit, System.ValueType nextEnumState)
	{
		if(curTime >= maxTime)
		{
			unit.ChangeState(nextEnumState);
		}
	}

	public void ChangeChaseState(EnemyController unit)
	{
		int enemyType = (int)unit.enemyType;

		switch(enemyType)
		{
			case 0:
				unit.ChangeState(EnemyController.EnemyState.MDefaultChase);
				break;

			case 1:
				unit.ChangeState(EnemyController.EnemyState.RDefaultChase);
				break;

			default:
				FDebug.Log("ERROR_ChangeChaseState()");
				return;
		}
	}
}
