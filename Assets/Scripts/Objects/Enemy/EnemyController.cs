using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static EnemyEffectManager;
using FMODUnity;
using System.Linq;
using UnityEngine.Events;

public class EnemyController : UnitFSM<EnemyController>, IFSM
{
	public enum EnemyState : int
	{
		Spawn,					//����
		Idle,					//���
		Default,
		MoveIdle,				//��� �� ���� �̵�
		Hitted,					//�ǰ�
		Death,                  //���

		ClusterSlow,
		ClusterChase,

		//Melee Default
		MDefaultChase,          //�߰�
		MDefaultAttack,         //����
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

	//[HideInInspector] public TestHPBar hpBar; //�ӽ�

	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;
	public EnemyType ThisEnemyType => enemyType;
	public bool isTutorialDummy = false;

	//animation name
	public readonly string moveAnimParam = "Move";          //�̵�
	public readonly string atkAnimParam = "Attack";         //����
	public readonly string ragnedAnimParam = "Ranged";
	public readonly string dashAnimParam = "Dash";			//�� �뽬
	public readonly string hitAnimParam = "Hit";            //�ǰ�
	public readonly string deadAnimParam = "Dead";			//���
	public readonly string playerTag = "Player";            //�÷��̾� �±� �̸�
	public readonly string matColorProperty = "_BaseColor";

	[Space(3)]
	[Header("Enemy Management")]
	[HideInInspector] public EnemyEffectManager effectManager;

	//clustering
	public bool isClusteringObj = false;
	[HideInInspector] public bool isClustering = false;
	[HideInInspector] public int clusterNum;
	[HideInInspector] public int individualNum = 0;
	[HideInInspector] public EnemyController clusterTarget;
	public float clusterDistance = 2.5f;

	//effect
	public List<EnemyEffectManager.Effect> effects;                           //����Ʈ ������
	public EnemyEffectManager.Effect hitEffect;
	public EnemyEffectManager.Effect hittedEffect;

	/*[HideInInspector] public List<GameObject> initiateEffects;
	[HideInInspector] public GameObject initiateHitEffect;*/

	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target;				//Attack target ����
	public Enemy enemyData;									//Enemy status ĳ��
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//���� �ݰ�
	public SphereCollider atkCollider;                      //Ÿ�� Collider

	public SkinnedMeshRenderer skinnedMeshRenderer;
	public Material material;
	public Material transparentMaterial;
	public Material unlitMaterial;
	[HideInInspector] public Material copyTMat;
	[HideInInspector] public Material copyUMat;


	[Space(3)]
	[Header("Spawn")]
	public float maxSpawningTime;                           //���� �ִ� �ð�
	[HideInInspector] public BoxCollider enemyCollider;     //�ǰ� Collider
	public GameObject spawnEffect;
	public float walkDistance = 3.0f;
	
	
	[Space(3)]
	[Header("Idle")]
	/*[HideInInspector] public bool isChasing = false;*/
	public float idleSetTime = 3f;                          //Default�� ��ȯ �� ��� �ð�

	[Space(3)]
	[Header("Default")]
	public float movePercentage = 5f;                       //MoveIdle/Idle �� ��ȯ ���� ��ġ

	[Space(3)]
	[Header("MoveIdle")]
	public float randMoveDistanceMin = 1.5f;
	public float randMoveDistanceMax = 3.0f;

	[Space(3)]
	[Header("Chase")]
	public float attackRange;								//���� ��ȯ ��Ÿ�
	public float attackChangeDelay;                         //���� �����
	public float turnSpeed = 15.0f;                         //ȸ�� ��ȯ �ӵ�

	[Space(3)]
	[Header("Attack")]
	[HideInInspector] public bool isAttackSuccess = false;
	public float projectileDistance;						//�߻�ü ��Ÿ�
	public GameObject rangedProjectile;						//�߻�ü ĳ��
	public float projectileSpeed;                           //�߻�ü �ӵ�

	public float powerReference1;							//���� ��
	public float powerReference2;


	[Space(3)]
	[Header("Hitted")]
	public float hitMaxTime = 2f;                           //�ǰ� �����
	public float hitColorChangeTime = 0.2f;
	public float hitPower = 450f;							//�ǰ� AddForce ��
	public Color damagedColor;                              //�ǰ� ��ȯ �÷���
	public EventReference hitSound;

	[Space(3)]
	[Header("Death")]
	public float deathDelay = 2.0f;

	[Space(3)] 
	[Header("Spawn Info & Event")]
	[HideInInspector] public UnityEvent disableEvent;
	
	private void Start()
	{
		effectManager = EnemyEffectManager.Instance;

		//hpBar = GetComponent<TestHPBar>(); //�ӽ�

		//Basic Set Up
		animator = GetComponent<Animator>();
		rigid = GetComponent<Rigidbody>();
		enemyCollider = GetComponent<BoxCollider>();
		navMesh = GetComponent<NavMeshAgent>();
		
		if (spawnEffect != null)
			spawnEffect.transform.parent = null;

		SetMaterial();

		if(chaseRange != null)
			chaseRange.enabled = false;

		unit = this;
		if (isTutorialDummy)
		{
			effectManager.CopyEffect(unit);
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
