using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : UnitFSM<EnemyController>, IFSM
{
	public enum EnemyState : int
	{
		Spawn,					//스폰
		Idle,					//대기
		Default,
		MoveIdle,				//대기 중 랜덤 이동
		Hitted,					//피격
		Death,					//사망

		//Melee Default
		MDefaultChase,          //추격
		MDefaultAttack,         //공격
		MDefaultAttack2nd,

		//Ranged Default
		RDefaultChase,	
		RDefaultBackMove,
		RDefaultAttack,

		//MinimalDefault
		MiniDefaultChase,
		MiniDefaultDelay,
		MiniDefaultAttack,
		MiniDefaultKnockback,
	}

	public enum EnemyType : int
	{
		MeleeDefault,
		RangedDefault,
		MinimalDefault,
	}

	[HideInInspector] public TestHPBar hpBar; //임시

	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;

	[Space(3)]
	[Header("Spawn")]				
	public float maxSpawningTime;  //스폰 최대 시간
	[HideInInspector] public BoxCollider enemyCollider;                      //피격 Collider
	public GameObject spawnEffect;


	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target;				//Attack target 지정
	public Enemy enemyData;									//Enemy status 캐싱
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//추적 반경
	public SphereCollider atkCollider;                      //타격 Collider


	[Space(3)]
	[Header("Idle")]
	/*[HideInInspector] public bool isChasing = false;*/
	public float idleSetTime = 3f;                          //Default로 변환 전 대기 시간

	[Space(3)]
	[Header("Default")]
	public float movePercentage = 5f;                       //MoveIdle/Idle 중 변환 랜덤 수치

	[Space(3)]
	[Header("MoveIdle")]
	public Transform transformParent;						//Hierarchy MoveIdle Transform 정리용
	[HideInInspector] public GameObject moveIdleSpot;       //MoveIdle 이동 타겟

	[Space(3)]
	[Header("Chase")]
	public float attackRange;								//공격 전환 사거리
	public float attackChangeDelay;                         //공격 딜레이
	public float turnSpeed = 15.0f;                         //회전 전환 속도

	[Space(3)]
	[Header("Attack")]
	public float projectileDistance;						//발사체 사거리
	public GameObject rangedProjectile;						//발사체 캐싱
	public float projectileSpeed;                           //발사체 속도

	public float powerReference1;							//돌진 등
	public float powerReference2;

	[Serializable]
	public struct Effects
	{
		public GameObject effect;
		public Transform effectPos;
		public GameObject effectParent;
	}

	public List<Effects> effects;                           //이펙트 프리팹
	[HideInInspector] public List<GameObject> initiateEffects;
	//[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;

	public Material whiteMaterial;                          //쫄 돌진 차징 머테리얼
	[HideInInspector] public Material copyWhiteMat;

	[Space(3)]
	[Header("Hitted")]
	public float hitMaxTime = 2f;                           //피격 딜레이
	//public float hitPower;									//피격 AddForce 값
	public Color damagedColor;								//피격 변환 컬러값

	public Material eMaterial;                              //머테리얼 복제용 캐싱
	[HideInInspector] public Material copyMat;
	public SkinnedMeshRenderer skinnedMeshRenderer;			//머테리얼 인덱스 캐싱

	//animation name
	public readonly string moveAnimParam = "Move";			//이동 애니 파라미터
	public readonly string atkAnimParam = "Attack";			//공격 애니 파라미터
	public readonly string hitAnimParam = "Hit";			//피격 애니 파라미터

	//tag name
	public readonly string playerTag = "Player";            //플레이어 태그 이름

	public readonly string matColorProperty = "_BaseColor";



	private void Start()
	{
		//임시 : 추후 삭제 에정, 크리틱 빌드를 위함
		this.gameObject.SetActive(false); 
		hpBar = GetComponent<TestHPBar>();

		//Basic Set Up
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		enemyCollider = GetComponent<BoxCollider>();
		navMesh = GetComponent<NavMeshAgent>();
		if(whiteMaterial != null)
			copyWhiteMat = new Material(whiteMaterial);
		if (eMaterial != null)
			copyMat = new Material(eMaterial);

		chaseRange.enabled = false;

		unit = this;
		SetUp(EnemyState.Spawn);

		for (int i = 0; i < effects.Count; i++)
		{
			initiateEffects.Add(GameObject.Instantiate(effects[i].effect, effects[i].effectParent == null ? null : effects[i].effectPos.transform));
			initiateEffects[i].SetActive(false);
		}

		//effectPoolManager = new ObjectPoolManager<Transform>(effects[0].effect, effects[0].effectParent);
	}


	protected override void Update()
	{
		base.Update();
	}

	public void DelayChangeState (float curTime, float maxTime, EnemyController unit, System.ValueType nextEnumState)
	{
		if(curTime >= maxTime)
		{
			unit.ChangeState(nextEnumState);
		}
	}

	public System.ValueType UnitChaseState(EnemyController unit)
	{
		int enemyType = (int)unit.enemyType;

		switch (enemyType)
		{
			case 0:
				return EnemyController.EnemyState.MDefaultChase;

			case 1:
				return EnemyController.EnemyState.RDefaultChase;

			case 2:
				return EnemyController.EnemyState.MiniDefaultChase;

			default:
				FDebug.Log("ERROR_ChangeChaseState()");
				return null;
		}
	}
}
