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
		Death,

		T1_Melee,
		T2_Ranged,
		T3_Laser,
		T4_Laser,
		T5_EnemySpawn,
		T6_Circle,
		T7_Trap,
	}

	[Header("CurrentState")]
	public BossState curState;
	public bool isActive = false;

	[Space(8)]
	[Header("Target ÁöÁ¤")]
	public UnitBase target;

	[Space(8)]
	[Header("Data cashing")]
	public BossData bossData;
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

	[Space(8)]
	[Header("Spawn Info & Event")]
	[HideInInspector] public UnityEvent disableEvent;

	private void Start()
	{
		unit = this;

		SetUp(BossState.SetUp);
	}

	protected override void Update()
	{
		base.Update();
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
