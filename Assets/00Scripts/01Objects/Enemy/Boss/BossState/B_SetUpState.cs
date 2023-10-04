using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[FSMState((int)BossController.BossState.SetUp)]
public class B_SetUpState : UnitState<BossController>
{
	public override void Begin(BossController unit)
	{
		unit.curState = BossController.BossState.SetUp;

		//Basic
		unit.target = GameObject.FindWithTag("Player").GetComponent<UnitBase>();
		unit.animator = unit.GetComponent<Animator>();
		unit.rigid = unit.GetComponent<Rigidbody>();
		unit.navMesh = unit.GetComponent<NavMeshAgent>();

		//Material
		unit.copyUMat = new Material(unit.unlitMaterial);
		unit.copyUMat.SetColor("_BaseColor", new Color(1.0f, 1.0f, 1.0f, 0f));
		unit.meshRenderer.material = unit.copyUMat;

		//Effect
		unit.effectController = ECManager.Instance.GetEffectManager(unit.effectSO);
		unit.currentEffectData = new EffectActiveData();
	}

	public override void End(BossController unit)
	{
	}

	public override void Update(BossController unit)
	{
		if (unit.isActive)
			unit.ChangeState(BossController.BossState.Chase);
	}

	public override void FixedUpdate(BossController unit)
	{
	}

	public override void OnCollisionEnter(BossController unit, Collision collision)
	{
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
	}
}
