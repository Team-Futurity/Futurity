using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.BetaSM)]
public class PlayerBetaPartState : PlayerSpecialMoveState<BasicActivePart>
{
	private readonly string BetaPartAnimKey = "Beta";
	private List<UnitBase> enemyList = new List<UnitBase>();

	private Transform betaEffect;

	private PlayerController pc;

	private TruncatedBoxCollider boxCollider;

	public override void Begin(PlayerController unit)
	{
		
	}

	public override void Update(PlayerController unit)
	{
		
	}

	public override void FixedUpdate(PlayerController unit)
	{
		
	}

	public override void End(PlayerController unit)
	{
		
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		
	}
}
