using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyEffectManager;
using FMODUnity;

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
		RDefaultDelay,

		//MinimalDefault
		MiniDefaultChase,
		MiniDefaultDelay,
		MiniDefaultAttack,
		MiniDefaultKnockback,

		//Cluster
		ClusterChase,
		ClusterSlow,

	}

	public enum EnemyType : int
	{
		MeleeDefault,
		RangedDefault,
		MinimalDefault,
	}

	//[HideInInspector] public TestHPBar hpBar; //임시

	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;

	//animation name
	public readonly string moveAnimParam = "Move";          //이동
	public readonly string atkAnimParam = "Attack";         //공격
	public readonly string dashAnimParam = "Dash";			//쫄 대쉬
	public readonly string hitAnimParam = "Hit";            //피격
	public readonly string deadAnimParam = "Dead";			//사망
	public readonly string playerTag = "Player";            //플레이어 태그 이름
	public readonly string matColorProperty = "_BaseColor";

	[Space(3)]
	[Header("Enemy Management")]
	public EnemyManager manager;

	//clustering
	public bool isClusteringObj = false;
	[HideInInspector] public bool isClustering = false;
	[HideInInspector] public int clusterNum;
	[HideInInspector] public int individualNum = 0;
	[HideInInspector] public EnemyController clusterTarget;
	public float clusterDistance = 2.5f;

	//effect
	public List<EnemyEffectManager.Effect> effects;                           //이펙트 프리팹
	public EnemyEffectManager.Effect hitEffect;
	public EnemyEffectManager.Effect hittedEffect;

	/*[HideInInspector] public List<GameObject> initiateEffects;
	[HideInInspector] public GameObject initiateHitEffect;*/



	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target;				//Attack target 지정
	public Enemy enemyData;									//Enemy status 캐싱
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//추적 반경
	public SphereCollider atkCollider;                      //타격 Collider

	public SkinnedMeshRenderer skinnedMeshRenderer;
	/*public Material material;
	[HideInInspector] public Material copyMat;*/
	public Material transparentMaterial;
	[HideInInspector] public Material copyTMat;


	[Space(3)]
	[Header("Spawn")]
	public float maxSpawningTime;                           //스폰 최대 시간
	[HideInInspector] public BoxCollider enemyCollider;     //피격 Collider
	public GameObject spawnEffect;
	public float walkDistance = 3.0f;


	[Space(3)]
	[Header("Idle")]
	/*[HideInInspector] public bool isChasing = false;*/
	public float idleSetTime = 3f;                          //Default로 변환 전 대기 시간

	[Space(3)]
	[Header("Default")]
	public float movePercentage = 5f;                       //MoveIdle/Idle 중 변환 랜덤 수치

	[Space(3)]
	[Header("MoveIdle")]
	public float randMoveDistanceMin = 1.5f;
	public float randMoveDistanceMax = 3.0f;

	[Space(3)]
	[Header("Chase")]
	public float attackRange;								//공격 전환 사거리
	public float attackChangeDelay;                         //공격 딜레이
	public float turnSpeed = 15.0f;                         //회전 전환 속도

	[Space(3)]
	[Header("Attack")]
	[HideInInspector] public bool isAttackSuccess = false;
	public float projectileDistance;						//발사체 사거리
	public GameObject rangedProjectile;						//발사체 캐싱
	public float projectileSpeed;                           //발사체 속도

	public float powerReference1;							//돌진 등
	public float powerReference2;


	[Space(3)]
	[Header("Hitted")]
	public float hitMaxTime = 2f;                           //피격 딜레이
	//public float hitPower;									//피격 AddForce 값
	public Color damagedColor;                              //피격 변환 컬러값
	public EventReference hitSound;

	[Space(3)]
	[Header("Death")]
	public float deathDelay = 2.0f;


	private void Awake()
	{
		manager = EnemyManager.Instance;
	}

	private void Start()
	{
		//hpBar = GetComponent<TestHPBar>(); //임시

		//Basic Set Up
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		enemyCollider = GetComponent<BoxCollider>();
		navMesh = GetComponent<NavMeshAgent>();
		if (transparentMaterial != null)
		{
			copyTMat = new Material(transparentMaterial);
			copyTMat.SetColor(matColorProperty, new Color(1, 1, 1, 0f));
			skinnedMeshRenderer.material = copyTMat;
		}
		if(spawnEffect != null)
		spawnEffect.transform.parent = null;

/*		if (material != null)
		{
			copyMat = new Material(material);
			unit.skinnedMeshRenderer.material = unit.copyMat;
		}*/

		manager.ActiveManagement(this);
		EnemyEffectManager.Instance.CopyEffect(this);
		chaseRange.enabled = false;

		//FDebug.Log(hittedEffect.indexNum);

		unit = this;
		SetUp(EnemyState.Spawn);
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

	public System.ValueType UnitChaseState()
	{
		switch ((int)enemyType)
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
