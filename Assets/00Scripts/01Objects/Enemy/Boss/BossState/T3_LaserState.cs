using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T3_Laser)]
public class T3_LaserState : B_PatternBase
{

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T3_Laser;
		unit.SetListEffectData(unit.attackEffectDatas, unit.attackTrigger.type3Colliders, EffectActivationTime.InstanceAttack, EffectTarget.Ground, false);
		unit.navMesh.enabled = true;
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
		distance = Vector3.Distance(unit.transform.position, unit.attackTrigger.type3StartPos.position);

		if (distance > 0.1f)
		{
			unit.transform.LookAt(unit.attackTrigger.type3StartPos.position);
			unit.navMesh.SetDestination(unit.attackTrigger.type3StartPos.position);
		}
		else
		{
			if (curTime > unit.curAttackData.attackDelay && !isAttackDelayDone)
			{
				unit.transform.rotation = Quaternion.Euler(Vector3.zero);
				unit.rigid.velocity = Vector3.zero;
				isAttackDelayDone = true;
			}

			if (isAttackDelayDone && !isAttackDone)
			{
				unit.animator.SetTrigger(unit.type2Anim);
				isAttackDone = true;
			}

			if (isAttackDone && curTime > unit.curAttackData.attackDelay + unit.curAttackData.attackSpeed + unit.curAttackData.attackAfterDelay)
			{
				unit.ChangeState(unit.nextState);
			}
		}
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.navMesh.enabled = false;
	}
}
