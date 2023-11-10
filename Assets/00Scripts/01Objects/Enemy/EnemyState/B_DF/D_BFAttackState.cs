using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;
using static UnityEngine.RuleTile.TilingRuleOutput;

[FSMState((int)EnemyState.D_BFAttack)]
public class D_BFAttackState : EnemyAttackBaseState
{
	private List<EffectActiveData> floorEffects = new List<EffectActiveData>();
	private List<EffectActiveData> atkEffects = new List<EffectActiveData>();

	public GameObject colliderParent;
	public static float zFarDistance;
	public static float flooringTiming = 0f;
	public static float atkTiming = 0f;
	public static float deActiveTiming = 0f;
	public static float attackSpeed = 0f;

	private bool atk1stDone = false;
	private bool atk2ndDone = false;
	private bool atk3rdDone = false;


	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);
		EffectSetting(unit);
		unit.test.transform.localPosition = new Vector3(0, 0, zFarDistance);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		if (!isAttack)
		{
			AttackAnim(unit, curTime, unit.attackBeforeDelay);
		}
		else
		{
			if (!atk1stDone && curTime > attackSpeed)
			{
				AttackProcessing(unit, 0);
				atk1stDone = true;
			}
			else if (!atk2ndDone && curTime > attackSpeed * 2)
			{
				AttackProcessing(unit, 1);
				atk2ndDone = true;
			}
			else if (!atk3rdDone && curTime > attackSpeed * 3)
			{
				AttackProcessing(unit, 2);
				atk3rdDone = true;
			}
			else
				unit.DelayChangeState(curTime, unit.attackingDelay, unit, unit.UnitChaseState());
		}

	}

	public override void End(EnemyController unit)
	{
		base.End(unit);
		atk1stDone = false;
		atk2ndDone = false;
		atk3rdDone = false;

		floorEffects.Clear();
		atkEffects.Clear();
	}

	private void EffectSetting(EnemyController unit)
	{
		for (int i = 0; i < unit.atkColliders.Count; i++)
		{
			EffectActiveData effectData = new EffectActiveData();
			effectData.activationTime = EffectActivationTime.InstanceAttack;
			effectData.target = EffectTarget.Ground;
			effectData.index = 0;
			effectData.position = unit.atkColliders[i].transform.position;
			effectData.rotation = unit.atkColliders[i].transform.rotation;
			effectData.parent = null;

			floorEffects.Add(effectData);
		}

		for (int i = 0; i < unit.atkColliders.Count; i++)
		{
			EffectActiveData effectData = new EffectActiveData();
			effectData.activationTime = EffectActivationTime.InstanceAttack;
			effectData.target = EffectTarget.Target;
			effectData.index = 0;
			effectData.position = unit.atkColliders[i].transform.position;
			effectData.rotation = unit.atkColliders[i].transform.rotation;
			effectData.parent = null;

			atkEffects.Add(effectData);
		}
	}

	private void AttackProcessing(EnemyController unit, int index)
	{
		FlooringAttackProcess atkProcess = new FlooringAttackProcess();

		//atkProcess.Setting(floorEffects[index], atkEffects[index], unit.atkColliders[index], flooringTiming, atkTiming, deActiveTiming, unit);
		atkProcess.StartProcess();
	}
}