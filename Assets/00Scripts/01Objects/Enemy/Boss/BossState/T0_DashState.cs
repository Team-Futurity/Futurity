using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T0_Dash)]
public class T0_DashState : B_PatternBase
{
	private float dashPower = 800f;

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T0_Dash;
	}

	public override void Update(BossController unit)
	{
		base.Update(unit);

		if(curTime > unit.curAttackData.attackDelay && !isAttackDelayDone)
		{
			targetPos = unit.target.transform.position;
			unit.transform.LookAt(targetPos);
			isAttackDelayDone = true;
		}

		if(isAttackDelayDone && !isAttackDone)
		{
			unit.attackTrigger.type0Collider.SetActive(true);
			unit.rigid.AddForce(unit.transform.forward * dashPower, ForceMode.Impulse);
			isAttackDone = true;
		}

		if(isAttackDone && curTime > unit.curAttackData.attackDelay + unit.curAttackData.attackSpeed)
		{
			unit.attackTrigger.type0Collider.SetActive(false);
			unit.rigid.velocity = Vector3.zero;
		}
		
		else if(isAttackDone && curTime > unit.curAttackData.attackDelay + unit.curAttackData.attackSpeed + unit.curAttackData.attackAfterDelay)
			unit.ChangeState(unit.nextState);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
		if (other.CompareTag("Player"))
		{
			DamageInfo info = new DamageInfo(unit.bossData, unit.target, unit.curAttackData.extraAttackPoint, unit.curAttackData.targetKnockbackPower);
			unit.bossData.Attack(info);
			unit.nextState = BossState.T1_Melee;
			unit.ChangeState(unit.nextState);
		}
	}
}
