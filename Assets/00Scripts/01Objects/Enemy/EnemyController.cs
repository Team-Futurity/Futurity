using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using System.Linq;
using UnityEngine.Events;

public class EnemyController : UnitFSM<EnemyController>, IFSM
{
	public enum EnemyState : int
	{
		Spawn,					//스폰
		Idle,					//대기
		Default,
		
		MoveIdle,				//대기 중 랜덤 이동
		Hitted,					//피격
		Death,                  //사망

		ClusterSlow,
		ClusterChase,

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

		//EliteDefault
		EliteDefaultChase,
		EliteMeleeAttack,
		EliteRangedAttack,

		//Tutorial
		TutorialIdle,

	}

	public enum EnemyType : int
	{
		MeleeDefault,
		RangedDefault,
		MinimalDefault,
		EliteDefault,
	}

	//[HideInInspector] public TestHPBar hpBar; //임시

	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;
	public EnemyType ThisEnemyType => enemyType;
	public bool isTutorialDummy = false;

	//animation name
	public readonly string moveAnimParam = "Move";          //이동
	public readonly string atkAnimParam = "Attack";         //공격
	public readonly string ragnedAnimParam = "Ranged";
	public readonly string dashAnimParam = "Dash";			//쫄 대쉬
	public readonly string hitAnimParam = "Hit";            //피격
	public readonly string deadAnimParam = "Dead";			//사망
	public readonly string playerTag = "Player";            //플레이어 태그 이름

	public readonly string matColorProperty = "_BaseColor";

	[Space(3)]
	[Header("Enemy Management")]
	public EnemyAnimationEvents animationEvents;
	public EffectController effectController;
	public EffectDatas effectSO;
	public EffectActiveData currentEffectData = new EffectActiveData();


	//clustering
	public bool isClusteringObj = false;
	[HideInInspector] public bool isClustering = false;
	[HideInInspector] public int clusterNum;
	[HideInInspector] public int individualNum = 0;
	[HideInInspector] public EnemyController clusterTarget;
	public float clusterDistance = 2.5f;

	[Space(3)]
	[Header("Reference")]
	/*[HideInInspector]*/ public UnitBase target;				//Attack target 지정
	public Enemy enemyData;									//Enemy status 캐싱
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//추적 반경
	public SphereCollider atkCollider;                      //타격 Collider

	public SkinnedMeshRenderer skinnedMeshRenderer;
	public Material material;
	public Material transparentMaterial;
	public Material unlitMaterial;
	[HideInInspector] public Material copyTMat;
	[HideInInspector] public Material copyUMat;


	[Space(3)]
	[Header("Spawn")]
	public float maxSpawningTime;                           //스폰 최대 시간
	[HideInInspector] public BoxCollider enemyCollider;     //피격 Collider
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
	public float hitColorChangeTime = 0.2f;
	public float hitPower = 450f;							//피격 AddForce 값
	public Color damagedColor;                              //피격 변환 컬러값
	public EventReference hitSound;

	[Space(3)]
	[Header("Death")]
	public float deathDelay = 2.0f;

	[Space(3)] 
	[Header("Spawn Info & Event")]
	[HideInInspector] public UnityEvent disableEvent;
	
	private void Start()
	{
		effectController = ECManager.Instance.GetEffectManager(effectSO);

		//hpBar = GetComponent<TestHPBar>(); //임시

		//Basic Set Up
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		enemyCollider = GetComponent<BoxCollider>();
		navMesh = GetComponent<NavMeshAgent>();

		SetMaterial();

		if (chaseRange != null)
			chaseRange.enabled = false;

		unit = this;
		if (isTutorialDummy)
		{
			SetUp(EnemyState.TutorialIdle);
		}
		else
			SetUp(EnemyState.Spawn);
	}

	protected override void Update()
	{
		base.Update();
	}

	public void SetMaterial()
	{
		if (unlitMaterial != null)
		{
			copyUMat = new Material(unlitMaterial);
			copyUMat.SetColor(matColorProperty, new Color(1.0f, 1.0f, 1.0f, 0f));
			skinnedMeshRenderer.material = copyUMat;
			
		}
		if(transparentMaterial != null)
		{
			copyTMat = new Material(transparentMaterial);
			copyTMat.SetColor(matColorProperty, new Color(0f, 0f, 0f, 0f));
		}
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
		switch (enemyType)
		{
			case EnemyType.MeleeDefault:
				return EnemyController.EnemyState.MDefaultChase;

			case EnemyType.RangedDefault:
				return EnemyController.EnemyState.RDefaultChase;

			case EnemyType.MinimalDefault:
				return EnemyController.EnemyState.MiniDefaultChase;

			case EnemyType.EliteDefault:
				return EnemyController.EnemyState.EliteDefaultChase;

			default:
				FDebug.Log("ERROR_ChangeChaseState()");
				return null;
		}
	}
	
	public void RegisterEvent(UnityAction eventFunc)
	{
		disableEvent.AddListener(eventFunc);
	}
	
	public void OnDisableEvent()
	{
		disableEvent?.Invoke();
	}
}
