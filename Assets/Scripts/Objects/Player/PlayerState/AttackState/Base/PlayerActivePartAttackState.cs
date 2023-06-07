using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActivePartAttackState<T> : PlayerAttackBaseState where T : ActivePartProccessor
{
	protected T proccessor;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);
	}

	public override void OnCollisionStay(PlayerController unit, Collision collision)
	{
		base.OnCollisionStay(unit, collision);
	}

	public virtual void SetActivePartData(T proccessor)
	{
		this.proccessor = proccessor;
	}
}
