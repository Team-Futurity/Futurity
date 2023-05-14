using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerController.PlayerState.Attack)]
public class PlayerAttackState : UnitState<PlayerController>
{
	// Animation Key
	protected readonly string NormalAttackAnimKey = "NormalAttack";
	protected readonly string AttackTriggerAnimKey = "AttackTrigger";
	protected readonly string MeleeAnimaKey = "Melee";

	// 임시 변수
	public float animRatio = 0.7f;

	// etc
	private float currentTime;
	private Transform effect;
	//private CameraController cam;
	protected AttackNode attackNode;

	public override void Begin(PlayerController pc)
	{
		/*if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		pc.animator.SetBool(NormalAttackAnimKey, true);
		pc.animator.SetTrigger(AttackTriggerAnimKey);
		pc.animator.SetFloat(MeleeAnimaKey, pc.curNode.animFloat);
		pc.curNode.Copy(pc.curNode);
		attackNode = pc.curNode;
		//effect = attackNode.effectPoolManager.ActiveObject(attackNode.effectPos.position, pc.transform.rotation);
		currentTime = 0;
		//cam.SetVibration(attackNode.shakeTime, attackNode.curveShakePower, attackNode.randomShakePower);
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime > attackNode.skillSpeed * animRatio)
		{
			pc.ChangeState(PlayerState.AttackDelay);
		}
		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		//임시
		pc.glove.SetActive(false);
		pc.rigid.velocity = Vector3.zero;

		PlayerAnimationEvents animEventEffect = pc.GetComponent<PlayerAnimationEvents>();
		FDebug.Log($"{animEventEffect.effect.name}가 존재합니다.");
		attackNode.effectPoolManager.DeactiveObject(animEventEffect.effect);
		pc.attackCollider.radiusCollider.enabled = false;
		pc.animator.SetBool(NormalAttackAnimKey, false);
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		if(other.CompareTag(unit.EnemyTag))
		{
			if (unit.attackCollider.IsInCollider(other.gameObject))
			{
				unit.playerData.Attack(other.GetComponent<UnitBase>());
			}
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
