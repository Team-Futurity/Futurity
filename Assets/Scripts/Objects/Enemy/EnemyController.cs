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
		Spawn,					//½ºÆù
		Idle,					//´ë±â
		Default,
<<<<<<< Updated upstream
		MoveIdle,				//ï¿½ï¿½ï¿½ ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½Ìµï¿½
		Hitted,					//ï¿½Ç°ï¿½
		Death,                  //ï¿½ï¿½ï¿½
=======
		MoveIdle,				//´ë±â Áß ·£´ý ÀÌµ¿
		Hitted,					//ÇÇ°Ý
		Death,                  //»ç¸Á
>>>>>>> Stashed changes

		ClusterSlow,
		ClusterChase,

		//Melee Default
		MDefaultChase,          //Ãß°Ý
		MDefaultAttack,         //°ø°Ý
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

	//[HideInInspector] public TestHPBar hpBar; //ÀÓ½Ã

	[Header("Enemy Parameter")]
	[SerializeField] private EnemyType enemyType;
	public EnemyType ThisEnemyType => enemyType;
	public bool isTutorialDummy = false;

	//animation name
<<<<<<< Updated upstream
	public readonly string moveAnimParam = "Move";          //ï¿½Ìµï¿½
	public readonly string atkAnimParam = "Attack";         //ï¿½ï¿½ï¿½ï¿½
	public readonly string ragnedAnimParam = "Ranged";
	public readonly string dashAnimParam = "Dash";			//ï¿½ï¿½ ï¿½ë½¬
	public readonly string hitAnimParam = "Hit";            //ï¿½Ç°ï¿½
	public readonly string deadAnimParam = "Dead";			//ï¿½ï¿½ï¿½
	public readonly string playerTag = "Player";            //ï¿½Ã·ï¿½ï¿½Ì¾ï¿½ ï¿½Â±ï¿½ ï¿½Ì¸ï¿½
=======
	public readonly string moveAnimParam = "Move";          //ÀÌµ¿
	public readonly string atkAnimParam = "Attack";         //°ø°Ý
	public readonly string ragnedAnimParam = "Ranged";
	public readonly string dashAnimParam = "Dash";			//ÂÌ ´ë½¬
	public readonly string hitAnimParam = "Hit";            //ÇÇ°Ý
	public readonly string deadAnimParam = "Dead";			//»ç¸Á
	public readonly string playerTag = "Player";            //ÇÃ·¹ÀÌ¾î ÅÂ±× ÀÌ¸§
>>>>>>> Stashed changes
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
	public List<EnemyEffectManager.Effect> effects;                           //ÀÌÆåÆ® ÇÁ¸®ÆÕ
	public EnemyEffectManager.Effect hitEffect;
	public EnemyEffectManager.Effect hittedEffect;

	/*[HideInInspector] public List<GameObject> initiateEffects;
	[HideInInspector] public GameObject initiateHitEffect;*/

	[Space(3)]
	[Header("Reference")]
	[HideInInspector] public UnitBase target;				//Attack target ÁöÁ¤
	public Enemy enemyData;									//Enemy status Ä³½Ì
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	public CapsuleCollider chaseRange;						//ÃßÀû ¹Ý°æ
	public SphereCollider atkCollider;                      //Å¸°Ý Collider

	public SkinnedMeshRenderer skinnedMeshRenderer;
	public Material material;
	public Material transparentMaterial;
	public Material unlitMaterial;
	[HideInInspector] public Material copyTMat;
	[HideInInspector] public Material copyUMat;


	[Space(3)]
	[Header("Spawn")]
	public float maxSpawningTime;                           //½ºÆù ÃÖ´ë ½Ã°£
	[HideInInspector] public BoxCollider enemyCollider;     //ÇÇ°Ý Collider
	public GameObject spawnEffect;
	public float walkDistance = 3.0f;
	
	
	[Space(3)]
	[Header("Idle")]
	/*[HideInInspector] public bool isChasing = false;*/
	public float idleSetTime = 3f;                          //Default·Î º¯È¯ Àü ´ë±â ½Ã°£

	[Space(3)]
	[Header("Default")]
	public float movePercentage = 5f;                       //MoveIdle/Idle Áß º¯È¯ ·£´ý ¼öÄ¡

	[Space(3)]
	[Header("MoveIdle")]
	public float randMoveDistanceMin = 1.5f;
	public float randMoveDistanceMax = 3.0f;

	[Space(3)]
	[Header("Chase")]
	public float attackRange;								//°ø°Ý ÀüÈ¯ »ç°Å¸®
	public float attackChangeDelay;                         //°ø°Ý µô·¹ÀÌ
	public float turnSpeed = 15.0f;                         //È¸Àü ÀüÈ¯ ¼Óµµ

	[Space(3)]
	[Header("Attack")]
	[HideInInspector] public bool isAttackSuccess = false;
	public float projectileDistance;						//¹ß»çÃ¼ »ç°Å¸®
	public GameObject rangedProjectile;						//¹ß»çÃ¼ Ä³½Ì
	public float projectileSpeed;                           //¹ß»çÃ¼ ¼Óµµ

	public float powerReference1;							//µ¹Áø µî
	public float powerReference2;


	[Space(3)]
	[Header("Hitted")]
	public float hitMaxTime = 2f;                           //ÇÇ°Ý µô·¹ÀÌ
	public float hitColorChangeTime = 0.2f;
	public float hitPower = 450f;							//ÇÇ°Ý AddForce °ª
	public Color damagedColor;                              //ÇÇ°Ý º¯È¯ ÄÃ·¯°ª
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

		//hpBar = GetComponent<TestHPBar>(); //ÀÓ½Ã

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
