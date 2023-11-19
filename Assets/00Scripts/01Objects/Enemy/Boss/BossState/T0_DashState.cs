using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T0_Dash)]
public class T0_DashState : B_PatternBase
{
	private float playerDistance = 0.5f;
	private EffectActiveData effectData = new EffectActiveData();
	public T0_DashState()
	{
		effectData.activationTime = EffectActivationTime.AfterDoingAttack;
		effectData.target = EffectTarget.Target;
		effectData.index = 0;
		effectData.position = new Vector3(0f, 1f, 0f);
		effectData.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
	}

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T0_Dash;
		ActiveAnimProcess(unit, unit.dashAnim);
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
			unit.bossData.EnableAttackTiming();
			unit.attackTrigger.type0Collider.SetActive(true);
			unit.ActiveDashEffect(effectData);
			unit.rigid.velocity = unit.transform.forward.normalized * unit.bossData.status.GetStatus(StatusType.DASH_SPEED).GetValue();
			//unit.rigid.AddForce(unit.transform.forward * unit.dashPower, ForceMode.Acceleration);
			isAttackDone = true;
		}

		if (isAttackDone && curTime > unit.curAttackData.attackDelay + unit.curAttackData.attackSpeed)
		{
			unit.rigid.velocity = Vector3.zero;
			unit.attackTrigger.type0Collider.SetActive(false);
		}

		if (isAttackDone && curTime > unit.curAttackData.attackDelay + unit.curAttackData.attackSpeed + unit.curAttackData.attackAfterDelay)
			unit.ChangeState(unit.nextState);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.animator.SetBool(unit.moveAnim, false);
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
		if (other.CompareTag("Player"))
		{
			DamageInfo info = new DamageInfo(unit.bossData, unit.target, unit.curAttackData.extraAttackPoint, unit.curAttackData.targetKnockbackPower);
			unit.bossData.Attack(info);

			unit.rigid.velocity = Vector3.zero;
			other.attachedRigidbody.velocity = Vector3.zero;
			unit.transform.position += (unit.transform.position - other.transform.position).normalized * playerDistance;
			/*unit.nextState = BossState.T1_Melee;
			unit.ChangeState(unit.nextState);*/
		}
	}
}
