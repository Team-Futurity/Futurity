using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyController : UnitFSM<EnemyController>, IFSM
{
	public GameObject test;


	#region Field

	[Space(3)]
	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;
	public EnemyType ThisEnemyType => enemyType;

	[Space(3)]
	[Header("Enemy Management")]
	public EffectController effectController;
	public EffectDatas effectSO;
	public EffectActiveData currentEffectData;
	public EffectKey currentEffectKey;

	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target = null;				//Attack target ����
	public Enemy enemyData;									//Enemy status ĳ��
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//���� �ݰ�
	public SphereCollider atkCollider;                      //Ÿ�� Collider
	[HideInInspector] public BoxCollider enemyCollider;     //�ǰ� Collider

	public SkinnedMeshRenderer skinnedMeshRenderer;
	public Material material;
	public Material unlitMaterial;
	[HideInInspector] public Material copyUMat;

	[Space(3)]
	[Header("Attack")]
	public float attackRange = 7f;
	public float attackingDelay = 4f;

	[Space(3)]
	[Header("Hitted")]
	public float hitPower = 450f;
	public Color damagedColor;
	[HideInInspector] public bool isInPlayer = false;


	[HideInInspector]public UnityEvent onDeathEvent;
	[HideInInspector] public UnityEvent disableEvent;
	public EventReference attackSound1;
	public EventReference attackSound2;
	public EventReference attackSound3;
	public EventReference hitSound;

	#region Animation name
	//animation name
	public readonly string moveAnimParam = "Move";          //�̵�
	public readonly string atkAnimParam = "Attack";         //����
	public readonly string ragnedAnimParam = "Ranged";
	public readonly string dashAnimParam = "Dash";          //�� �뽬
	public readonly string hitAnimParam = "Hit";            //�ǰ�
	public readonly string deadAnimParam = "Dead";          //���
	public readonly string playerTag = "Player";            //�÷��̾� �±� �̸�

	public readonly string matColorProperty = "_BaseColor";
	#endregion

	#endregion

	private void Start()
	{
		if(effectSO)
			effectController = ECManager.Instance.GetEffectManager(effectSO);
		currentEffectData = new EffectActiveData();
		currentEffectKey = null;

		animator = GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody>();
		enemyCollider = GetComponent<BoxCollider>();
		navMesh = GetComponent<NavMeshAgent>();

		SetMaterial();

		if (chaseRange != null)
			chaseRange.enabled = false;

		unit = this;
		if (this.enemyType == EnemyType.TutorialDummy)
			SetUp(EnemyState.TutorialIdle);
		else
			SetUp(EnemyState.Spawn);
	}

	#region Enemy controller methods
	public void SetMaterial()
	{
		if (unlitMaterial != null)
		{
			copyUMat = new Material(unlitMaterial);
			copyUMat.SetColor(matColorProperty, new Color(1.0f, 1.0f, 1.0f, 0f));
			skinnedMeshRenderer.material = copyUMat;
			
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
				return EnemyState.MDefaultChase;

			case EnemyType.RangedDefault:
				return EnemyState.RDefaultChase;

			case EnemyType.MinimalDefault:
				return EnemyState.MiniDefaultChase;

			case EnemyType.EliteDefault:
				return EnemyState.EliteDefaultChase;

			default:
				FDebug.Log("ERROR_ChangeChaseState()");
				return null;
		}
	}

	public void SettingProjectile()
	{
		UnitState<EnemyController> s = null;
		GetState(EnemyState.RDefaultAttack, ref s);
		((RDefaultAttackState)s).SetProjectile(GetComponentInChildren<Projectile>().gameObject);
	}

	#endregion


	#region Production

	public void ActiveEnemy()
	{
		if (target == null)
			this.ChangeState(EnemyState.Idle);
		else
			this.ChangeState(UnitChaseState());
	}

	public void DeActiveEnemy()
	{
		this.ChangeState(EnemyState.None);
	}

	public void MoveToPosition(Vector3 targetPos)
	{
		UnitState<EnemyController> s = null;
		GetState(EnemyState.AutoMove, ref s);
		((EnemyAutoMoveState)s).SetTarget(targetPos);
		this.ChangeState(EnemyState.AutoMove);
	}

	#endregion

	public void RegisterEvent(UnityAction eventFunc)
	{
		disableEvent.AddListener(eventFunc);
	}
	
	public void OnDisableEvent()
	{
		disableEvent?.Invoke();
	}
}
