using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using System.Linq;
using UnityEngine.Events;

public class EnemyController : UnitFSM<EnemyController>, IFSM
{
	public GameObject test;

	[Space(3)]
	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;
	public EnemyType ThisEnemyType => enemyType;

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
	public EnemyAnimationEvents animationEvents;
	public EffectController effectController;
	public EffectDatas effectSO;
	public EffectActiveData currentEffectData;

	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target;				//Attack target ����
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


	private void Start()
	{
		if(effectSO)
			effectController = ECManager.Instance.GetEffectManager(effectSO);
		currentEffectData = new EffectActiveData();

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
	
	public void RegisterEvent(UnityAction eventFunc)
	{
		disableEvent.AddListener(eventFunc);
	}
	
	public void OnDisableEvent()
	{
		disableEvent?.Invoke();
	}
}
