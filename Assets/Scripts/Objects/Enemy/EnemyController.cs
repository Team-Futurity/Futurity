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

	[SerializeField] private EnemyType enemyType;

	//spawn
	private bool isSpawning;								//���� ���ΰ� ����
	private float curSpawningTime;							
	[SerializeField] private float maxSpawningTime = 2f;	//���� �ִ� �ð�
	private BoxCollider enemyCollider;						//�ǰ� Collider

	//Reference
	[HideInInspector] public UnitBase target;				//Attack target ����
	public Enemy enemyData;									//Enemy status ĳ��
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;

	public CapsuleCollider chaseRange;						//���� �ݰ�
	public SphereCollider atkCollider;						//Ÿ�� Collider

	//Idle Properties
	/*[HideInInspector] public bool isChasing = false;*/
	public float idleSetTime = 3f;							//Default�� ��ȯ �� ��� �ð�

	//Default Properties
	public float movePercentage = 5f;						//MoveIdle/Idle �� ��ȯ ���� ��ġ

	//MoveIdle Properties
	public Transform transformParent;						//Hierarchy MoveIdle Transform ������
	[HideInInspector] public GameObject moveIdleSpot;		//MoveIdle �̵� Ÿ��

	//Chase Properties
	public float attackRange;								//���� ��ȯ ��Ÿ�
	public float attackChangeDelay;                         //���� ������
	public float turnSpeed = 15.0f;							//ȸ�� ��ȯ �ӵ�

	//Attack Properties
	public float projectileDistance;						//�߻�ü ��Ÿ�
	public GameObject rangedProjectile;						//�߻�ü ĳ��
	public float projectileSpeed;                           //�߻�ü �ӵ�

	public float powerReference1;							//���� ��
	public float powerReference2;

	public Transform effectPos;								//����Ʈ ��� ��ġ
	public GameObject effectPrefab;							//����Ʈ ������
	public GameObject effectParent;                         //����Ʈ ��� �θ�
	/*	[HideInInspector] public ObjectPoolManager<Transform> effectPoolManager;*/
	
	public Material whiteMaterial;							//�� ���� ��¡ ���׸���

	//Hitted Properties
	public float hitMaxTime = 2f;                           //�ǰ� ������
	public float hitPower;									//�ǰ� AddForce ��
	public Color damagedColor;								//�ǰ� ��ȯ �÷���

	public Material eMaterial;								//���׸��� ������ ĳ��
	public SkinnedMeshRenderer skinnedMeshRenderer;			//���׸��� �ε��� ĳ��

	//animation name
	public readonly string moveAnimParam = "Move";			//�̵� �ִ� �Ķ����
	public readonly string atkAnimParam = "Attack";			//���� �ִ� �Ķ����
	public readonly string hitAnimParam = "Hit";			//�ǰ� �ִ� �Ķ����

	//tag name
	public readonly string playerTag = "Player";			//�÷��̾� �±� �̸�



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
