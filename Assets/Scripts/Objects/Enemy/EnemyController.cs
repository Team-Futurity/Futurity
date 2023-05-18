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
		MDefaultAttack2nd,

		//Ranged Default
		RDefaultChase,	
		RDefaultBackMove,
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
	[HideInInspector] public UnitBase target;
	public Enemy enemyData;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;

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
	public Transform transformParent;
	[HideInInspector] public GameObject moveIdleSpot;

	//Attack Properties
	public float attackSetTime = 2f;
	public float attackDelayTime = 1.3f;
	public float rangedDistance;
	public float projectileDistance;
	public GameObject rangedProjectile;
	public float projectileSpeed;

	public Transform effectPos;
	public GameObject effectPrefab;
	public GameObject effectParent;
/*	[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;*/

	//Chase Properties


	//BackMove Properties


	//Hitted Properties
	public float hitMaxTime = 2f;
	public Color defaultColor;
	public Color damagedColor;

	public Material eMaterial;
	public SkinnedMeshRenderer skinnedMeshRenderer;

	//animation name
	public readonly string moveAnimParam = "Move";
	public readonly string atkAnimParam = "Attack";
	public readonly string hitAnimParam = "Hit";

	//tag name
	public readonly string playerTag = "Player";



	private void Start()
	{
		//Basic Set Up
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		enemyCollider = GetComponent<BoxCollider>();
		if(atkCollider != null)
			atkCollider.enabled = false;
		chaseRange.enabled = false;
		enemyCollider.enabled = false;
/*		effectPoolManager = new ObjectPoolManager<Transform>(effectPrefab, effectParent);*/

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
			}
			else
			{
				unit.ChangeState(EnemyState.Default);
				enemyCollider.enabled = true;
				chaseRange.enabled = true;
				curSpawningTime = 0f;
				isSpawning = false;
			}
		}
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
