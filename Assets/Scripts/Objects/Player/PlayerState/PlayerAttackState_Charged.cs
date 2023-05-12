using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor.TestTools.CodeCoverage;
using UnityEngine;
using UnityEngine.Rendering;
using static PlayerController;

[FSMState((int)PlayerController.PlayerState.ChargedAttack)]
public class PlayerAttackState_Charged : PlayerAttackState
{
	// Constants
	private readonly float LengthMarkIncreasing = 200;
	private readonly float AttackSTIncreasing = 1;
	private readonly float LevelStandard = 1;
	private readonly string WallTag = "Wall";

	// Variables
	private float playerOriginalSpeed;
	private int currentLevel;
	private float currentTime;

	// others
	private Coroutine rushCoroutine;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		playerOriginalSpeed = unit.playerData.Speed;
		unit.playerData.SetSpeed(unit.playerData.Speed * 0.5f);
		currentTime = 0;
		currentLevel = 0;
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);
		unit.playerData.SetSpeed(playerOriginalSpeed);
		unit.rigid.velocity = Vector3.zero;
	}

	public override void FixedUpdate(PlayerController unit)
	{
		base.FixedUpdate(unit);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{
		base.OnCollisionEnter(unit, collision);

		if (collision.gameObject.CompareTag(WallTag))
		{
			unit.isRush = false;
			unit.ChangeState(PlayerState.AttackDelay);
		}
	}

	public override void Update(PlayerController unit)
	{
		int level = (int)(currentTime / LevelStandard) + 1;

		if(currentLevel != level)
		{
			currentLevel = level;
		}

		// 버튼이 Release 됐다면
		if (unit.specialIsReleased)
		{
			unit.specialIsReleased = false;
			unit.isRush = true;

			float attackST = unit.curNode.attackST + level * AttackSTIncreasing;
			float attackLengthMark = unit.curNode.attackLengthMark + level * LengthMarkIncreasing;

			if (rushCoroutine == null)
			{
				rushCoroutine = unit.StartCoroutine(unit.ChargedAttackProc(attackST, attackLengthMark));
			}
		}

		currentTime += Time.deltaTime;
	}
}
