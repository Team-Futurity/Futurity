using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[FSMState((int)PlayerController.PlayerState.BasicPart)]
public class PlayerBasicPartState : PlayerActivePartAttackState<BasicActivePart>
{
	private const float maxAngle = 360;
	private List<UnitBase> enemies = new List<UnitBase>();

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		enemies.Clear();
		unit.attackCollider.radiusCollider.enabled = true;
		unit.attackCollider.SetCollider(maxAngle, proccessor.minRange * PlayerController.cm2m);
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);

		float radius = Mathf.Lerp(unit.attackCollider.radius, proccessor.maxRange, proccessor.duration / Time.deltaTime);

		unit.attackCollider.SetCollider(maxAngle, radius);

		if (currentTime >= proccessor.duration)
		{
			unit.attackCollider.SetCollider(maxAngle, proccessor.maxRange * PlayerController.cm2m);
			unit.ChangeState(PlayerController.PlayerState.Idle);
		}
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

		if(other.CompareTag(unit.EnemyTag))
		{
			enemies.Add(other.GetComponent<UnitBase>());
		}
	}

	public override void OnTriggerExit(PlayerController unit, Collider other)
	{
		base.OnTriggerExit(unit, other);
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);
	}

	public override void OnCollisionStay(PlayerController unit, Collision collision)
	{
		base.OnCollisionStay(unit, collision);
	}
}
