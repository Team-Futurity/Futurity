using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.BetaSM)]
public class PlayerBetaPartState : PlayerSpecialMoveState<BetaActivePart>
{
	private enum BetaProcess
	{
		NONE = 0,
		FIRST_PHASE = 1,
		SECOND_PHASE = 2,
		THIRD_PHASE = 3,
		END = 4
	}

	// Common
	private List<UnitBase> enemyList = new List<UnitBase>();

	private PlayerController pc;

	private TruncatedBoxCollider boxColl;
	private TruncatedCapsuleCollider capsuleColl;

	private BetaProcess currentProcess;
	private bool isPlaying = false;

	private readonly string BetaAnimKey = "IsBetaActive";
	
	// Phase 1
	private float firstMaxAngle = .0f;
	private float firstMinSize = .0f;
	private float firstRadius;
	private float firstDamage = .0f;

	private Transform firstEffect;


	// Phase 2
	private float secondMaxAngle = .0f;
	private float secondMinSize = .0f;
	private float secondRadius;
	private float secondDamage = .0f;

	// Phase 3

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);

		Debug.Log("Beta State ����");

		// Ȥ�� �� ���� ������ ����
		enemyList.Clear();

		pc = unit;

		firstMinSize = proccessor.firstRadius;
		secondRadius = proccessor.secondRadius;

		if (unit.attackColliderChanger.GetCollider(ColliderType.Capsule) is TruncatedCapsuleCollider capsuleCollider)
		{
			capsuleColl = capsuleCollider;
			capsuleColl.SetCollider(firstMaxAngle, firstMinSize);
		}

		capsuleColl.ColliderReference.enabled = true;
		
		currentProcess = BetaProcess.FIRST_PHASE;
		isPlaying = true;
		// Animation Play
		unit.animator.SetBool(BetaAnimKey, true);
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);

		// �� ������
		if (currentTime == Time.deltaTime || !isPlaying) return;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController unit)
	{
		pc.animator.SetBool(BetaAnimKey, false);
		Debug.Log("Beta State�� ���");
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		base.OnTriggerEnter(unit, other);

		if (other.CompareTag(unit.EnemyTag))
		{
			other.TryGetComponent<UnitBase>(out var enemy);
			if (enemy == null) { return; }

			enemyList.Add(enemy);
		}
	}

	// ����Ʈ ���ÿ� ������ ó��
	public void OnFirstPhase()
	{
		Debug.Log("On First Phase");
		
		// Collider Open
		capsuleColl.SetCollider(firstMaxAngle, firstMinSize);
		// Effect Create

		// Damage Add
		AddDamageEnemy(pc);
	}

	public void OnFirstPhaseEnded()
	{
		// Next Process Set
		currentProcess = BetaProcess.SECOND_PHASE;
		capsuleColl.SetCollider(0, 0);
	}

	public void OnSecondPhase()
	{
		Debug.Log("On Second Phase");
		
		// Collider
		capsuleColl.SetCollider(secondMaxAngle, secondRadius);
		
		// Effect

		// Damage Add
		AddDamageEnemy(pc);
	}

	public void OnSecondPhaseEnded()
	{
		// Next Process Set
		currentProcess = BetaProcess.THIRD_PHASE;
	}

	public void OnThirdPhase()
	{
		Debug.Log("On Third Phase");
		
		// Collider �þ�� �ϱ� 
		// Collider ������ �÷��̾ �ٶ󺸴� �������� �����ϱ�
		
		// ���� �ð� ������ ���, ������ Max�� �����ϱ�
		// Collider ����.
		// ������ ����
	}

	public void OnThirdPhaseEnded()
	{
		// Next Process Set
		currentProcess = BetaProcess.END;
	}

	public void OnEnded()
	{
		pc.ChangeState(PlayerState.Idle);
	}

	// Enemy���� ������ ����
	private void AddDamageEnemy(PlayerController unit)
	{
		foreach (var enemy in enemyList)
		{
			DamageInfo info = new DamageInfo(unit.playerData, enemy, 1);

			// Cycle ���� �������� �ٸ��� ����
			var currentDamage = currentProcess == BetaProcess.FIRST_PHASE ? firstDamage :
				currentProcess == BetaProcess.SECOND_PHASE ? secondDamage : 300;
			
			Debug.Log(currentProcess);
			
			info.SetDamage(currentDamage);
			enemy.Hit(info);
		}

		enemyList.Clear();
	}
}