using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerController.PlayerState.Attack)]
public class PlayerAttackState : UnitState<PlayerController>
{
	private float currentTime;
	private Transform effect;
	//private CameraController cam;
	private AttackNode attackNode;

	public override void Begin(PlayerController pc)
	{
/*		if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		pc.isAttacking = true;
		pc.animator.SetTrigger("MeleeT");
		pc.animator.SetFloat("Melee", pc.curNode.animFloat);
		pc.curNode.Copy(pc.curNode);
		pc.isComboState = true;
		pc.comboCurTime = 0f;
		attackNode = pc.curNode;
		//effect = attackNode.effectPoolManager.ActiveObject(attackNode.effectPos.position, pc.transform.rotation);
		currentTime = 0;
		pc.attackCollider.radiusCollider.enabled = true;
		pc.attackCollider.SetCollider(attackNode.skillAngle, attackNode.skillRange);
		//cam.SetVibration(attackNode.shakeTime, attackNode.curveShakePower, attackNode.randomShakePower);

		AudioManager.instance.PlayOneShot(attackNode.attackSound, pc.transform.position);

		//임시
		pc.glove.SetActive(true);
		pc.rigid.velocity = Vector3.zero;
		pc.rigid.velocity = pc.transform.forward * attackNode.moveDistance;
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime > attackNode.skillSpeed)
		{
			pc.BackToPreviousState();
		}
		currentTime += Time.deltaTime;
		return;
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
		pc.isAttacking = false;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		if(other.CompareTag("Enemy"))
		{
			if (unit.attackCollider.IsInCollider(other.gameObject))
			{
				unit.playerData.Attack(other.GetComponent<UnitBase>());
			}
			FDebug.Log("EnemyCollision");
		}
		FDebug.Log("test");
	}

}
