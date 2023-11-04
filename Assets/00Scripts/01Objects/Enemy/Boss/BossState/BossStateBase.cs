using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateBase : UnitState<BossController>
{
	protected float curTime = 0f;
	protected float distance = .0f;

	public override void Begin(BossController unit)
	{
	}

	public override void Update(BossController unit)
	{
		curTime += Time.deltaTime;
	}


	public override void End(BossController unit)
	{
		curTime = 0f;
	}

	public override void FixedUpdate(BossController unit)
	{
	}

	public override void OnCollisionEnter(BossController unit, Collision collision)
	{
	}

	public override void OnTriggerEnter(BossController unit, Collider other)
	{
	}
}
