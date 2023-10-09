using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using UnityEngine.Events;

public class BossController : UnitFSM<BossController>, IFSM
{
	public enum BossState : int
	{
		SetUp,

		Idle,
		Chase,
		Hit,
		Death,

		T1_Melee,
		T2_Ranged,
		T3_Move,
		T3_Laser,
		T4_Laser,
		T5_EnemySpawn,
		T6_Circle,
		T7_Trap,
	}
	public bool isActive = false;

	[Space(8)]
	[Header("State")]
	public BossState curState;
	public Phase curPhase;
	[HideInInspector] public BossState nextPattern;
	[HideInInspector] public BossState afterType467Pattern;

	[Space(8)]
	[Header("Target ÁöÁ¤")]
	public UnitBase target;

	[Space(8)]
	[Header("Data cashing")]
	public Boss bossData;
	[HideInInspector] public Animator animator;
	[HideInInspector] public Rigidbody rigid;
	[HideInInspector] public NavMeshAgent navMesh;

	[Space(8)]
	[Header("Material cashing")]
	public SkinnedMeshRenderer meshRenderer;
	public Material material;
	public Material unlitMaterial;
	[HideInInspector] public Material copyUMat;

	[Space(8)]
	[Header("Effect cashing")]
	[HideInInspector] public EffectController effectController;
	public EffectDatas effectSO;
	[HideInInspector] public EffectActiveData currentEffectData;
	[HideInInspector] public List<EffectActiveData> listEffectData;

	[Space(8)]
	[Header("Spawn Info & Event")]
	[HideInInspector] public UnityEvent disableEvent;


	[Space(8)]
	[Header("Pattern")]
	public BossActiveDatas activeDataSO;
	public BossPhaseDatas phaseDataSO;

	//setting value
	public float targetDistance = 9f;
	public float meleeDistance = 4f;
	public int maxTypeCount = 5;
	[HideInInspector] public float skillAfterDelay;
	[HideInInspector] public float type467MaxTime;
	[HideInInspector] public float type5MaxTime;

	public bool isActivateType467 = false;
	public bool isActivateType5 = false;

	[HideInInspector] public float type4Percentage;
	[HideInInspector] public float type6Percentage;
	[HideInInspector] public float type7Percentage;

	//current value
	[HideInInspector] public int typeCount = 0;
	[HideInInspector] public float cur467Time;
	[HideInInspector] public float cur5Time = 0;

	//Animator Parameter
	public readonly string moveAnim = "Move";
	public readonly string hitAnim = "Hit";
	public readonly string deathAnim = "Death";
	public readonly string type1Anim = "Type1";
	public readonly string type2Anim = "Type2";
	public readonly string type3Anim = "Type3";
	public readonly string type4Anim = "Type4";
	public readonly string type5Anim = "Type5";
	public readonly string type6Anim = "Type6";
	public readonly string type7Anim = "Type7";

	[Space(8)]
	[Header("Attack Collider")]
	public GameObject Type1Collider;
	public GameObject Type2Collider;
	public List<GameObject> Type3Colliders;
	public Transform type3StartPos;
	public List<GameObject> Type4Colliders;
	public List<SpawnerManager> Type5Manager;
	public List<GameObject> Type6Colliders;
	public List<GameObject> Type7Colliders;



	private void Start()
	{
		unit = this;

		SetUp(BossState.SetUp);
	}

	protected override void Update()
	{
		base.Update();

		if(isActive)
		{
			if (!isActivateType467)
			{
				cur467Time += Time.deltaTime;
				if (cur467Time > type467MaxTime)
					isActivateType467 = true;
			}
			if (!isActivateType5)
			{
				cur5Time += Time.deltaTime;
				if (cur5Time > type5MaxTime)
					isActivateType5 = true;
			}
		}
	}

	public void DelayChangeState(float curTime, float maxTime, BossController.BossState nextState)
	{
		if(curTime > maxTime)
			unit.ChangeState(nextState);
	}

	public void DeActiveAttacks(List<GameObject> list)
	{
		listEffectData.Clear();
		if (list.Count > 0)
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetParent(null, true);
				list[i].SetActive(false);
			}
	}

	public void SetEffectData(List<GameObject> list, EffectActivationTime activationTime, EffectTarget target)
	{
		for (int i = 0; i < list.Count; i++)
		{
			EffectActiveData data = new EffectActiveData();
			data.activationTime = activationTime;
			data.target = target;
			data.position = list[i].transform.position;
			data.rotation = list[i].transform.rotation;
			data.parent = null;
			data.index = 0;
			listEffectData.Add(data);
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
