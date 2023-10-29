using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FSMState((int)PlayerState.BetaSM)]
public class PlayerBetaPartState : PlayerSpecialMoveState<BetaActivePart>
{
	// Common
	private List<UnitBase> enemyList = new List<UnitBase>();

	private PlayerController pc;

	private TruncatedBoxCollider boxColl;
	private TruncatedCapsuleCollider capsuleColl;

	private ColliderBase currentCollider;

	// Phase 1 -> 2 : Animation Ended
	// Phase 2 -> 3 : Animation Ended
	// Phase 3 : MaxSize
	private int skillCycle = 0;
	
	// Phase 1
	private float firstMaxAngle = .0f;
	private float firstMinSize = .0f;
	private float firstRadius;
	private float firstDamage = .0f;

	private Transform firstEffect;

	private readonly string FirstBetaAnimKey = "T";
	
	// Phase 2
	private float secondMaxAngle = .0f;
	private float secondMinSize = .0f;
	private float secondRadius;
	private float secondDamage = .0f;
	
	private readonly string SecondBetaAnimKey = "TT";
	
	// Phase 3
	
	
	// Collider를 만들고 Player의 X축 중앙 값으로 Collider를 가져오는 것이 나을듯
	
	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		
		// 혹시 모를 이전 데이터 제거
		enemyList.Clear();
		
		// TimelineManager에서 CutScene Event 실행 (여기서)
		
		pc = unit;

		firstMinSize = proccessor.firstRadius * MathPlus.cm2m;
		
		// 맨 처음 시작 시에는 1타 전용 
		if (unit.attackColliderChanger.GetCollider(ColliderType.Box) is TruncatedCapsuleCollider capsuleCollider)
		{
			capsuleColl = capsuleCollider;
			
			currentCollider = capsuleColl;
			currentCollider.SetCollider(firstMaxAngle, firstMinSize);
		}
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);

		// 왜 쓴거지
		if (currentTime == Time.deltaTime) return;

		if (skillCycle == 1)
		{
			OnFirstPhase();			
		}
		else if (skillCycle == 2)
		{
			pc.ChangeState(PlayerState.Idle);
		}
		else if (skillCycle == 3)
		{
			// Phase 3
		}
		else if (skillCycle == 4)
		{
			// Ended
		}
		
		
		// 기점을 시작으로 진행하였음
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

	// 이펙트 동시에 데미지 처리
	private void OnFirstPhase()
	{
		// Collider Open
		currentCollider.SetCollider(firstMaxAngle, firstRadius);
		// Effect Create

		// Damage Add
		AddDamageEnemy(pc);
		

		skillCycle += 1;
	}

	private void OnSecondPhase()
	{
		currentCollider.SetCollider(secondMaxAngle, secondRadius);
		
		skillCycle += 1;
	}

	private void OnThirdPhase()
	{
		//currentCollider.setC
		skillCycle += 1;
	}

	// Enemy에게 데미지 전달
	private void AddDamageEnemy(PlayerController unit)
	{
		foreach (var enemy in enemyList)
		{
			DamageInfo info = new DamageInfo(unit.playerData, enemy, 1);

			// Cycle 마다 데미지가 다르기 때문
			var currentDamage = 
				(skillCycle == 1) ? firstDamage : (skillCycle == 2) ? secondDamage : 30;
			
			info.SetDamage(currentDamage);
			enemy.Hit(info);
		}
		
		enemyList.Clear();
	}
}
