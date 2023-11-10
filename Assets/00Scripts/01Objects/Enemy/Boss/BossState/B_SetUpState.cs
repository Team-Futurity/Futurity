using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[FSMState((int)BossState.SetUp)]
public class B_SetUpState : BossStateBase
{
	public override void Begin(BossController unit)
	{
		unit.curState = BossState.SetUp;
		
		//Effect
		unit.effectController = ECManager.Instance.GetEffectManager(unit.effectSO);
		unit.currentEffectData = new EffectActiveData();
		unit.attackEffectDatas = new List<EffectActiveData>();

		//Material
		unit.copyUMat = new Material(unit.unlitMaterial);
		unit.copyUMat.SetColor("_BaseColor", new Color(1.0f, 1.0f, 1.0f, 0f));
		for(int i = 0; i < unit.meshRenderers.Count; i++)
		{
			unit.meshRenderers[i].materials = new Material[2] { unit.material, unit.copyUMat };
		}

		//Basic
		unit.target = GameObject.FindWithTag("Player").GetComponent<UnitBase>();
		unit.animator = unit.GetComponentInChildren<Animator>();
		unit.rigid = unit.GetComponent<Rigidbody>();
		unit.navMesh = unit.GetComponent<NavMeshAgent>();
		unit.curPhase = Phase.Phase1;
		unit.nextState = BossState.Idle;
		unit.navMesh.speed = unit.bossData.status.GetStatus(StatusType.SPEED).GetValue();

		unit.navMesh.enabled = false;
	}

	public override void Update(BossController unit)
	{
		if (unit.isActive)
			unit.ChangeState(unit.nextState);
	}
}
