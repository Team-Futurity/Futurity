using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyState.MiniDefaultDelay)]
public class MiniDefaultDelayState : StateBase
{
	private Color defaultColor = new Color(1, 1, 1, 0);
	private Color setColor = new Color(1, 1, 1, 0.15f);
	private Color refColor = new Color(1, 1, 1, 0.15f);

	private float delayTime = 1.5f;

	public override void Begin(EnemyController unit)
	{
		unit.copyUMat.SetColor(unit.matColorProperty, setColor);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		if (refColor.a < 1.0f)
			refColor.a += curTime * 0.01f;
		unit.copyUMat.SetColor(unit.matColorProperty, refColor);
		unit.DelayChangeState(curTime, delayTime, unit, EnemyState.MiniDefaultAttack);
	}

	public override void End(EnemyController unit)
	{
		curTime = 0f;
		refColor = setColor;
		unit.copyUMat.SetColor(unit.matColorProperty, defaultColor);
	}
}
