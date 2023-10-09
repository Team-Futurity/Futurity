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
		
		//Effect
		unit.effectController = ECManager.Instance.GetEffectManager(unit.effectSO);
		unit.currentEffectData = new EffectActiveData();
		unit.listEffectData = new List<EffectActiveData>();

		//Material
		unit.copyUMat = new Material(unit.unlitMaterial);
		unit.copyUMat.SetColor("_BaseColor", new Color(1.0f, 1.0f, 1.0f, 0f));
		unit.meshRenderer.material = unit.copyUMat;

		//Basic
		unit.target = GameObject.FindWithTag("Player").GetComponent<UnitBase>();
		unit.animator = unit.GetComponentInChildren<Animator>();
		unit.rigid = unit.GetComponent<Rigidbody>();
		unit.navMesh = unit.GetComponent<NavMeshAgent>();
		unit.nextPattern = BossController.BossState.Chase;
		unit.afterType467Pattern = BossController.BossState.Chase;

		unit.type467MaxTime = unit.phaseDataSO.GetType467TImerValue(Phase.Phase1);
		unit.type5MaxTime = unit.phaseDataSO.GetType5TImerValue(Phase.Phase1);
		unit.skillAfterDelay = 3f;
		unit.navMesh.enabled = false;

		unit.type3StartPos.SetParent(null, true);
		unit.Type5Manager.gameObject.transform.SetParent(null, true);

		unit.Type1Collider.SetActive(false);
		unit.Type2Collider.SetActive(false);
		AttackSetting(unit.Type3Colliders);
		AttackSetting(unit.Type4Colliders);
		AttackSetting(unit.Type6Colliders);
		AttackSetting(unit.Type7Colliders);
		/*unit.DeActiveAttacks(unit.Type3Colliders);
		unit.DeActiveAttacks(unit.Type4Colliders);
		unit.DeActiveAttacks(unit.Type6Colliders);
		unit.DeActiveAttacks(unit.Type7Colliders);*/
	}

	public override void End(BossController unit)
	{
	}

	public override void Update(BossController unit)
	{
		if (unit.isActive)
			unit.ChangeState(BossController.BossState.T3_Move);
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

	public void AttackSetting(List<GameObject> list)
	{
		if (list.Count > 0)
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.SetParent(null, true);
				list[i].SetActive(false);
			}
	}
}
