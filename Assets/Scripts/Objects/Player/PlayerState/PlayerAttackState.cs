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
	protected AttackNode attackNode;

	public override void Begin(PlayerController pc)
	{
		/*if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		pc.animator.SetBool("NormalAttack", true);
		pc.animator.SetTrigger("AttackTrigger");
		FDebug.Log("Attack IN");
		pc.animator.SetFloat("Melee", pc.curNode.animFloat);
		pc.curNode.Copy(pc.curNode);
		attackNode = pc.curNode;
		//effect = attackNode.effectPoolManager.ActiveObject(attackNode.effectPos.position, pc.transform.rotation);
		currentTime = 0;
		//cam.SetVibration(attackNode.shakeTime, attackNode.curveShakePower, attackNode.randomShakePower);
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime > attackNode.skillSpeed * 0.7f)
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
		pc.animator.SetBool("NormalAttack", false);
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

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
