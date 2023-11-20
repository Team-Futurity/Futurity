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
	private List<FlooringAttackProcess> attackProcesses;

	private bool atk1stDone = false;
	private bool atk2ndDone = false;
	private bool atk3rdDone = false;

	public override void Begin(EnemyController unit)
	{
		base.Begin(unit);

		//FDebug.Log("dddd");
		unit.test.transform.localPosition = new Vector3(0, 0.6f, unit.D_BFData.data.zFarDistance);
		//FDebug.Log(floorEffects.Count);
	}

	public override void Update(EnemyController unit)
	{
		
		curTime += Time.deltaTime;
		if (!isAttack)
		{
			unit.transform.LookAt(unit.target.transform.position);
			EffectSetting(unit);
			ProcessSetting(unit);
			AttackAnim(unit, curTime, unit.attackBeforeDelay);
		}
		else if(isAttack)
		{
			if (!atk1stDone && curTime > 1.0f +  unit.D_BFData.data.attackSpeed)
			{
				Attack(0);
				atk1stDone = true;
			}
			else if (!atk2ndDone && curTime > 1.0f + unit.D_BFData.data.attackSpeed * 2)
			{
				Attack(1);
				atk2ndDone = true;
			}
			else if (!atk3rdDone && curTime > 1.0f + unit.D_BFData.data.attackSpeed * 3)
			{
				Attack(2);
				atk3rdDone = true;
			}
			else
				unit.DelayChangeState(curTime, unit.attackingDelay, unit, unit.UnitChaseState());
		}

	}

	public override void End(EnemyController unit)
	{
		base.End(unit);
		floorEffects.Clear();
		atkEffects.Clear();
		attackProcesses.Clear();
		atk1stDone = false;
		atk2ndDone = false;
		atk3rdDone = false;
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
			effectData.rotation = Quaternion.Euler(new Vector3(-90f, 0, 0));
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

	private void ProcessSetting(EnemyController unit)
	{
		attackProcesses = new List<FlooringAttackProcess>();

		for (int i = 0; i < unit.atkColliders.Count; i++)
		{
			GameObject obj = new GameObject("DBF_Process" + i);
			obj.AddComponent<FlooringAttackProcess>();
			attackProcesses.Add(obj.GetComponent<FlooringAttackProcess>());
			attackProcesses[i].Setting(floorEffects[i], atkEffects[i], unit.atkColliders[i], unit.D_BFData.data.flooringTiming, unit.D_BFData.data.atkEffectTiming, unit.D_BFData.data.atktTiming, unit.D_BFData.data.deActiveTiming, i, unit, null);
		}
	}
	private void Attack(int index)
	{
		attackProcesses[index].StartProcess();
	}
}