using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)EnemyController.EnemyState.MiniDefaultDelay)]
public class MiniDefaultDelayState : UnitState<EnemyController>
{
	private float curTime = .0f;
	private Material copyWhite;
	private Color defaultColor = new Color(1, 1, 1, 0);
	private Color refColor = Color.white;

	public override void Begin(EnemyController unit)
	{
		if(copyWhite == null)
		{
			copyWhite = new Material(unit.whiteMaterial);
		}
		unit.skinnedMeshRenderer.material = copyWhite;
		copyWhite.SetColor("_BaseColor", Color.white);
	}

	public override void Update(EnemyController unit)
	{
		curTime += Time.deltaTime;
		if (refColor.a > 0)
			refColor.a -= curTime * 0.01f;
		copyWhite.SetColor("_BaseColor", refColor);
		unit.DelayChangeState(curTime, unit.chaseDelayTime, unit, EnemyController.EnemyState.MiniDefaultAttack);
	}

	public override void FixedUpdate(EnemyController unit)
	{

	}

	public override void End(EnemyController unit)
	{
		curTime = 0f;
		refColor = Color.white;
		copyWhite.SetColor("_BaseColor", defaultColor);
	}

	public override void OnTriggerEnter(EnemyController unit, Collider other)
	{

	}

	public override void OnCollisionEnter(EnemyController unit, Collision collision)
	{

	}
}
