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

	// Phase 1
	private float firstMaxAngle = .0f;
	private float firstMinSize = .0f;
	private float firstRadius;
	private float firstDamage = .0f;

	private Transform firstEffect;

	private readonly string FirstBetaAnimKey = "TestBetaAnim";

	// Phase 2
	private float secondMaxAngle = .0f;
	private float secondMinSize = .0f;
	private float secondRadius;
	private float secondDamage = .0f;

	private readonly string SecondBetaAnimKey = "TT";

	// Phase 3

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);

		Debug.Log("Beta State 진입");

		// 혹시 모를 이전 데이터 제거
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
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);

		// 왜 쓴거지
		if (currentTime == Time.deltaTime || !isPlaying) return;
		
		switch (currentProcess)
		{
			case BetaProcess.NONE:
				break;

			case BetaProcess.FIRST_PHASE:
				isPlaying = false;
				
				OnFirstPhase(unit);

				break;

			case BetaProcess.SECOND_PHASE:
				isPlaying = false;
				
				OnSecondPhase(unit);

				break;

			case BetaProcess.THIRD_PHASE:
				isPlaying = false;
				
				OnThirdPhase();

				break;

			case BetaProcess.END:
				isPlaying = false;
				
				OnEnded();
				
				break;
		}
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController unit)
	{
		Debug.Log("Beta State을 벗어남");
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

	// 이펙트 동시에 데미지 처리
	private void OnFirstPhase(PlayerController unit)
	{
		Debug.Log("On First Phase");
		
		// Collider Open
		capsuleColl.SetCollider(firstMaxAngle, firstMinSize);
		// Effect Create
		// unit.animator.SetBool(FirstBetaAnimKey, true);

		// Damage Add
		AddDamageEnemy(unit);
		
		OnFirstPhaseEnded(unit);
	}

	private void OnFirstPhaseEnded(PlayerController unit)
	{
		// Next Process Set
		currentProcess = BetaProcess.SECOND_PHASE;
		unit.animator.SetBool(FirstBetaAnimKey, false);
		
		capsuleColl.SetCollider(0, 0);
		
		isPlaying = true;
	}

	private void OnSecondPhase(PlayerController unit)
	{
		Debug.Log("On Second Phase");
		
		// Collider
		capsuleColl.SetCollider(secondMaxAngle, secondRadius);
		
		// Animation
		// Effect

		// Damage Add
		AddDamageEnemy(unit);
		
		OnSecondPhaseEnded(unit);
	}

	private void OnSecondPhaseEnded(PlayerController unit)
	{
		// Next Process Set
		currentProcess = BetaProcess.THIRD_PHASE;

		isPlaying = true;
	}

	private void OnThirdPhase()
	{
		Debug.Log("On Third Phase");
		
		// Collider 늘어나게 하기 
		// Collider 방향을 플레이어가 바라보는 방향으로 설정하기
		
		// 실행 시간 지났을 경우, 데이터 Max로 세팅하기
		// Collider 적용.
		// 데미지 전달
		
		// 애니메이션 끝났을 경우, OnThirdPhaseEnded 호출하기
	}

	private void OnThirdPhaseEnded()
	{
		// Next Process Set
		currentProcess = BetaProcess.END;
		
	}

	private void OnEnded()
	{
		pc.ChangeState(PlayerState.Idle);
	}

	// Enemy에게 데미지 전달
	private void AddDamageEnemy(PlayerController unit)
	{
		Debug.Log("Add Damage Enemy Start");
		
		Debug.Log(enemyList.Count);
		
		foreach (var enemy in enemyList)
		{
			DamageInfo info = new DamageInfo(unit.playerData, enemy, 1);

			// Cycle 마다 데미지가 다르기 때문
			var currentDamage = currentProcess == BetaProcess.FIRST_PHASE ? firstDamage :
				currentProcess == BetaProcess.SECOND_PHASE ? secondDamage : 300;
			
			Debug.Log(currentProcess);
			
			info.SetDamage(currentDamage);
			enemy.Hit(info);
		}

		enemyList.Clear();
	}
}