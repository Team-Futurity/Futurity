using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultDelay)]
public class MiniDefaultDelayState : UnitState<EnemyController>
{
	private float curTime = .0f;
	private Color defaultColor = new Color(1, 1, 1, 0);
	private Color setColor = new Color(1, 1, 1, 0.15f);
	private Color refColor = new Color(1, 1, 1, 0.15f);

	public override void Begin(EnemyController unit)
	{
		//FDebug.Log("Mini delay begin");

		unit.skinnedMeshRenderer.material = unit.copyUMat;

		unit.copyUMat.SetColor(unit.matColorProperty, setColor);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		if (refColor.a < 1.0f)
			refColor.a += curTime * 0.01f;
		unit.copyUMat.SetColor(unit.matColorProperty, refColor);
		unit.DelayChangeState(curTime, unit.attackChangeDelay, unit, EnemyController.EnemyState.MiniDefaultAttack);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		//FDebug.Log("Mini delay end");
		curTime = 0f;
		refColor = setColor;
		unit.copyUMat.SetColor(unit.matColorProperty, defaultColor);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
