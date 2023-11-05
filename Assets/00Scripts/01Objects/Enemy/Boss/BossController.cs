using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;
using UnityEngine.Events;

public class BossController : UnitFSM<BossController>, IFSM
{
	public bool isActive = false;
	[HideInInspector] public bool isDead = false;
	[HideInInspector] public bool isPhase2EventDone = false;

	[Space(8)]
	[Header("State")]
	public BossState curState;
	public Phase curPhase;
	/*[HideInInspector]*/ public BossState nextState;
	/*[HideInInspector]*/ public BossState previousState;

	#region Base Parameter
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
	public List<SkinnedMeshRenderer> meshRenderers;
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

	#endregion

	[Space(8)]
	[Header("Pattern")]
	public BossActiveDatas activeDataSO;
	public BossPhaseDatas phaseDataSO;
	[HideInInspector] public BossAttackData curAttackData;
	public AttackColliders attackTrigger;

	public float chaseDistance = 7.0f;


	#region Animator Parameter
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
	#endregion


	[Space(8)]
	[Header("Hit info")]
	public float hitMaxTime = 0.1f;
	public float hitColorChangeTime = 0.2f;
	public float hitPower = 450f;
	public Color damagedColor;



	private void Start()
	{
		unit = this;

		SetUp(BossState.SetUp);
	}

	#region methods

	public void DelayChangeState(float curTime, float maxTime, BossState nextState)
	{
		if(curTime > maxTime)
			unit.ChangeState(nextState);
	}

	public void SetEffectData(List<GameObject> list, EffectActivationTime activationTime, EffectTarget target, bool isParent)
	{
		for (int i = 0; i < list.Count; i++)
		{
			EffectActiveData data = new EffectActiveData();
			data.activationTime = activationTime;
			data.target = target;
			data.position = list[i].transform.position;
			data.rotation = list[i].transform.rotation;
			if (isParent)
				data.parent = list[i].gameObject;
			else
				data.parent = null;
			data.index = 0;
			listEffectData.Add(data);
		}
	}
	public void ActiveEffect(int activeIndex = 0)
	{
		EffectActiveData data = currentEffectData;
		EffectKey key = effectController.ActiveEffect(data.activationTime, data.target, data.position, data.rotation, data.parent, data.index, activeIndex, false);

		var particles = key.EffectObject.GetComponent<ParticleActiveController>();

		if (particles != null)
		{
			particles.Initialize(effectController, key);
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
	#endregion
}
