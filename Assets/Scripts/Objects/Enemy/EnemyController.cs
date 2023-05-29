using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : UnitFSM<EnemyController>, IFSM
{
	public enum EnemyState : int
	{
		Spawn,					//����
		Idle,					//���
		Default,
		MoveIdle,				//��� �� ���� �̵�
		Hitted,					//�ǰ�
		Death,					//���

		//Melee Default
		MDefaultChase,          //�߰�
		MDefaultAttack,         //����
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

	[HideInInspector] public TestHPBar hpBar; //�ӽ�

	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;

	[Space(3)]
	[Header("Spawn")]				
	public float maxSpawningTime;  //���� �ִ� �ð�
	[HideInInspector] public BoxCollider enemyCollider;                      //�ǰ� Collider
	public GameObject spawnEffect;


	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target;				//Attack target ����
	public Enemy enemyData;									//Enemy status ĳ��
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//���� �ݰ�
	public SphereCollider atkCollider;                      //Ÿ�� Collider


	[Space(3)]
	[Header("Idle")]
	/*[HideInInspector] public bool isChasing = false;*/
	public float idleSetTime = 3f;                          //Default�� ��ȯ �� ��� �ð�

	[Space(3)]
	[Header("Default")]
	public float movePercentage = 5f;                       //MoveIdle/Idle �� ��ȯ ���� ��ġ

	[Space(3)]
	[Header("MoveIdle")]
	public Transform transformParent;						//Hierarchy MoveIdle Transform ������
	[HideInInspector] public GameObject moveIdleSpot;       //MoveIdle �̵� Ÿ��

	[Space(3)]
	[Header("Chase")]
	public float attackRange;								//���� ��ȯ ��Ÿ�
	public float attackChangeDelay;                         //���� ������
	public float turnSpeed = 15.0f;                         //ȸ�� ��ȯ �ӵ�

	[Space(3)]
	[Header("Attack")]
	public float projectileDistance;						//�߻�ü ��Ÿ�
	public GameObject rangedProjectile;						//�߻�ü ĳ��
	public float projectileSpeed;                           //�߻�ü �ӵ�

	public float powerReference1;							//���� ��
	public float powerReference2;

	[Serializable]
	public struct Effects
	{
		public GameObject effect;
		public Transform effectPos;
		public GameObject effectParent;
	}

	public List<Effects> effects;                           //����Ʈ ������
	[HideInInspector] public List<GameObject> initiateEffects;
	//[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;

	public Material whiteMaterial;                          //�� ���� ��¡ ���׸���
	[HideInInspector] public Material copyWhiteMat;

	[Space(3)]
	[Header("Hitted")]
	public float hitMaxTime = 2f;                           //�ǰ� ������
	//public float hitPower;									//�ǰ� AddForce ��
	public Color damagedColor;								//�ǰ� ��ȯ �÷���

	public Material eMaterial;                              //���׸��� ������ ĳ��
	[HideInInspector] public Material copyMat;
	public SkinnedMeshRenderer skinnedMeshRenderer;			//���׸��� �ε��� ĳ��

	//animation name
	public readonly string moveAnimParam = "Move";			//�̵� �ִ� �Ķ����
	public readonly string atkAnimParam = "Attack";			//���� �ִ� �Ķ����
	public readonly string hitAnimParam = "Hit";			//�ǰ� �ִ� �Ķ����

	//tag name
	public readonly string playerTag = "Player";            //�÷��̾� �±� �̸�

	public readonly string matColorProperty = "_BaseColor";



	private void Start()
	{
		//�ӽ� : ���� ���� ����, ũ��ƽ ���带 ����
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
