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
	
	
	// Collider�� ����� Player�� X�� �߾� ������ Collider�� �������� ���� ������
	
	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);
		
		// Ȥ�� �� ���� ������ ����
		enemyList.Clear();
		
		// TimelineManager���� CutScene Event ���� (���⼭)
		
		pc = unit;

		firstMinSize = proccessor.firstRadius * MathPlus.cm2m;
		
		// �� ó�� ���� �ÿ��� 1Ÿ ���� 
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

		// �� ������
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
		
		
		// ������ �������� �����Ͽ���
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

	// ����Ʈ ���ÿ� ������ ó��
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

	// Enemy���� ������ ����
	private void AddDamageEnemy(PlayerController unit)
	{
		foreach (var enemy in enemyList)
		{
			DamageInfo info = new DamageInfo(unit.playerData, enemy, 1);

			// Cycle ���� �������� �ٸ��� ����
			var currentDamage = 
				(skillCycle == 1) ? firstDamage : (skillCycle == 2) ? secondDamage : 30;
			
			info.SetDamage(currentDamage);
			enemy.Hit(info);
		}
		
		enemyList.Clear();
	}
}
