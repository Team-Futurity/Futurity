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
	[HideInInspector] public bool isDead = false;

	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target = null;				//Attack target 지정
	public Enemy enemyData;									//Enemy status 캐싱
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//추적 반경
	public SphereCollider atkCollider;                      //타격 Collider
	public List<GameObject> atkColliders = new List<GameObject>();
	[HideInInspector] public BoxCollider enemyCollider;     //피격 Collider

	public SkinnedMeshRenderer skinnedMeshRenderer;
	public Material material;
	public Material unlitMaterial;
	public Material deadMaterial;
	[HideInInspector] public Material copyUMat;
	[HideInInspector] public Material copyDMat;

	[Space(3)]
	[Header("Spawn")]
	public Vector3 spawnEffectPos = new Vector3(0f, 0f, 0f);
	public Vector3 spawnEffectScale = new Vector3(1f, 1f, 1f);

	[Space(3)]
	[Header("Chase")]
	public float beforeChaseDelay = 1f;

	[Space(3)]
	[Header("Attack")]
	public float attackRange = 7f;
	public float attackBeforeDelay;
	public float attackingDelay = 4f;
	public D_BFScriptableObj D_BFData;

	[Space(3)]
	[Header("Hitted")]
	public float hitDelay = 0.4f;
	public float deathDelay = 0.4f;
	public float hitPower = 450f;
	public Color damagedColor;
	[HideInInspector] public bool isInPlayer = false;
	public int skipFrameCountBeforeStop;
	[HideInInspector] public int stopFrameCount;
	[HideInInspector] public float knockbackPower;


	[HideInInspector]public UnityEvent onDeathEvent;
	[HideInInspector] public UnityEvent disableEvent;
	public EventReference attackSound1;
	public EventReference attackSound2;
	public EventReference attackSound3;
	public EventReference hitSound;

	#region Animation name
	//animation name
	public readonly string moveAnimParam = "Move";          //이동
	public readonly string atkAnimParam = "Attack";         //공격
	public readonly string ragnedAnimParam = "Ranged";
	public readonly string dashAnimParam = "Dash";          //쫄 대쉬
	public readonly string hitFAnimParam = "HitF";            //피격
	public readonly string hitBAnimParam = "HitB";            //피격
	public readonly string deadAnimParam = "Dead";          //사망
	public readonly string playerTag = "Player";            //플레이어 태그 이름

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
			case EnemyType.M_CF:
				return EnemyState.MDefaultChase;

			case EnemyType.D_LF:
				return EnemyState.RDefaultChase;

			case EnemyType.T_DF:
				return EnemyState.MiniDefaultChase;

			case EnemyType.E_DF:
				return EnemyState.EliteDefaultChase;

			case EnemyType.D_BF:
				return EnemyState.D_BFChase;

			case EnemyType.M_JF:
				return EnemyState.M_JFChase;

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

	public void SetMaterial()
	{
		if (unlitMaterial != null)
		{
			copyUMat = new Material(unlitMaterial);
			copyUMat.SetColor(matColorProperty, new Color(1.0f, 1.0f, 1.0f, 0f));
			skinnedMeshRenderer.material = copyUMat;

		}
		if (deadMaterial != null)
		{
			copyDMat = new Material(deadMaterial);
			copyDMat.SetFloat("_distortion", 1.0f);
		}
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
