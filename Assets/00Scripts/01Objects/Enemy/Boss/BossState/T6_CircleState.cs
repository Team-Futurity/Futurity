using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)BossController.BossState.T6_Circle)]
public class T6_CircleState : B_PatternBase
{
	int randx;
	int randz;

	public override void Begin(BossController unit)
	{
		base.Begin(unit);
		unit.curState = BossController.BossState.T6_Circle;
		unit.nextPattern = unit.afterType467Pattern;
		for (int i = 0; i < unit.Type6Colliders.Count; i++)
		{
			randx = Random.Range(-6, 6);
			randz = Random.Range(-6, 6);
			unit.Type6Colliders[i].transform.position = new Vector3(randx, unit.transform.position.y, randz);
		}
		unit.SetEffectData(unit.Type6Colliders, EffectActivationTime.MoveWhileAttack, EffectTarget.Target, true);
		unit.animator.SetTrigger(unit.type6Anim);
	}
	public override void Update(BossController unit)
	{
		base.Update(unit);
	}

	public override void End(BossController unit)
	{
		base.End(unit);
		unit.DeActiveAttacks(unit.Type6Colliders);
	}
}
