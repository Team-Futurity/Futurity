using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossState.T6_Circle)]
public class T6_CircleState : B_PatternBase
{
	private Vector2 randomPos;
	private float attackRadius = 7f;

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossState.T6_Circle;
		for (int i = 0; i < unit.attackTrigger.type6Colliders.Count; i++)
		{
			randomPos = Random.insideUnitCircle * attackRadius;
			unit.attackTrigger.type6Colliders[i].transform.position = new Vector3(randomPos.x, 0, randomPos.y) + unit.transform.position;
		}
		unit.SetEffectData(unit.attackTrigger.type6Colliders, EffectActivationTime.MoveWhileAttack, EffectTarget.Target, true);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
		if (curTime > unit.curAttackData.attackDelay && !isAttackDelayDone)
		{
			isAttackDelayDone = true;
		}

		if (isAttackDelayDone && !isAttackDone)
		{
			unit.animator.SetTrigger(unit.type6Anim);
			isAttackDone = true;
		}

		if (isAttackDone && curTime > unit.curAttackData.attackDelay + unit.curAttackData.attackSpeed + unit.curAttackData.attackAfterDelay)
		{
			unit.ChangeState(unit.nextState);
		}
	}

	public override void End(BossController unit)
	{
		base.End(unit);
	}
}
