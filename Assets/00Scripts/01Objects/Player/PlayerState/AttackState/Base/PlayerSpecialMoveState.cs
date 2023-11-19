using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialMoveState<T> : PlayerAttackBaseState where T : SpecialMoveProcessor
{
	protected T proccessor;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);

		unit.rmController.SetRootMotion("ActivePart");
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);

		unit.rigid.velocity = Vector3.zero;
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);

		SendAttackEndMessage(unit);
		unit.comboGaugeSystem.ResetComboGauge();
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

	private void SendAttackEndMessage(PlayerController unit)
	{
		string msg = unit.GetInputData(PlayerInputEnum.SpecialMove, true, GetType().ToString(), "Complete").inputMsg;
		unit.attackEndEvent.Invoke(msg);
	}

}
