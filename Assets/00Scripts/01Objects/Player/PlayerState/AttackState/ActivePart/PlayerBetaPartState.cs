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

	private readonly string BetaAnimKey = "BetaTrigger";
	
	
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
	private float thirdCollWidth = .0f;
	private float thirdCollHeight = .0f;
	private float thirdDamage = .0f;

	public override void Begin(PlayerController unit)
	{
		base.Begin(unit);

		Debug.Log("Beta State ����");

		enemyList.Clear();

		pc = unit;
		
		// Set Inspector Data
		firstMinSize = proccessor.firstRadius;
		firstMaxAngle = proccessor.firstMaxAngle;
		firstRadius = proccessor.firstRadius;
		firstDamage = proccessor.firstDamage;

		secondMaxAngle = proccessor.secondMaxAngle;
		secondDamage = proccessor.secondDamage;
		secondRadius = proccessor.secondRadius;

		thirdCollHeight = proccessor.thirdMaxHeight;
		thirdCollWidth = proccessor.thirdMaxWdith;
		thirdDamage = proccessor.thirdDamage;

		if (unit.attackColliderChanger.GetCollider(ColliderType.Capsule) is TruncatedCapsuleCollider capsuleCollider)
		{
			capsuleColl = capsuleCollider;
			capsuleColl.SetCollider(firstMaxAngle, firstMinSize);
		}
		
		if (unit.attackColliderChanger.GetCollider(ColliderType.Box) is TruncatedBoxCollider boxCollider)
		{
			boxColl = boxCollider;
			boxColl.SetCollider(thirdCollWidth, thirdCollHeight);
		}

		capsuleColl.ColliderReference.enabled = true;
		boxColl.ColliderReference.enabled = false;
		
		currentProcess = BetaProcess.FIRST_PHASE;
		isPlaying = true;
		
		// Animation Play
		TimelineManager.Instance.EnableCutScene(ECutSceneType.ACTIVE_BETA);
		capsuleColl.SetCollider(firstMaxAngle, firstMinSize);
	}

	public override void Update(PlayerController unit)
	{
		base.Update(unit);

		if (currentTime == Time.deltaTime || !isPlaying) return;
	}

	public override void End(PlayerController unit)
	{
		base.End(unit);
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
		// Collider Open
		// Effect Create
		proccessor.firstAttackObjectPool.ActiveObject(proccessor.firstEffectPos.position, proccessor.firstEffectPos.rotation).
			GetComponent<ParticleController>().Initialize(proccessor.firstAttackObjectPool);
		// Damage Add
		AddDamageEnemy(pc, firstDamage);
	}

	public void OnFirstPhaseEnded()
	{
		// Next Process Set
		currentProcess = BetaProcess.SECOND_PHASE;
		capsuleColl.SetCollider(0, 0);
	}

	public void OnSecondPhase()
	{
		// Collider
		capsuleColl.SetCollider(secondMaxAngle, secondRadius);
		
		// Effect
		proccessor.secondAttackObjectPool.ActiveObject(proccessor.secondEffectPos.position, proccessor.secondEffectPos.rotation).
			GetComponent<ParticleController>().Initialize(proccessor.secondAttackObjectPool);;
		
		// Damage Add
		AddDamageEnemy(pc, secondDamage);
	}

	public void OnSecondPhaseEnded()
	{
		// Next Process Set
		currentProcess = BetaProcess.THIRD_PHASE;
		
		capsuleColl.ColliderReference.enabled = false;
		boxColl.ColliderReference.enabled = true;
		
		// Collider Swap
		capsuleColl.SetCollider(0, 0);
		boxColl.SetCollider(thirdCollWidth / 2, thirdCollHeight);
	}

	public void OnThirdPhase()
	{
		proccessor.thirdAttackObjectPool.ActiveObject(proccessor.thirdEffectPos.position, proccessor.thirdEffectPos.rotation).
		GetComponent<ParticleController>().Initialize(proccessor.thirdAttackObjectPool);
		
		// ������ ����
		AddDamageEnemy(pc, thirdDamage);
	}

	public void OnThirdPhaseEnded()
	{
		currentProcess = BetaProcess.END;
	}

	public void OnEnded()
	{
		pc.ChangeState(PlayerState.Idle);
	}

	// Enemy���� ������ ����
	private void AddDamageEnemy(PlayerController unit, float damage)
	{
		foreach (var enemy in enemyList)
		{
			DamageInfo info = new DamageInfo(unit.playerData, enemy, 1);

			info.SetDamage(damage);
			enemy.Hit(info);
			
			Debug.Log("���� ���� ����!");
		}

		enemyList.Clear();
	}
}