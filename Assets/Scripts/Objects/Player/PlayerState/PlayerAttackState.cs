using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

//[FSMState((int)PlayerController.PlayerState.Attack)]
public class PlayerAttackState : UnitState<PlayerController>
{
	// Animation Key
	protected readonly string AttackTriggerAnimKey = "AttackTrigger";
	protected readonly string AttackTypeAnimaKey = "Combo";

	// etc
	private float currentTime;
	private Transform effect;
	//private CameraController cam;
	protected AttackNode attackNode;

	protected PlayerAttackState(string attackTriggerKey, string attackTypeKey)
	{
		AttackTriggerAnimKey = attackTriggerKey;
		AttackTypeAnimaKey = attackTypeKey;
	}

	public override void Begin(PlayerController pc)
	{
		/*if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		/*pc.animator.SetBool(IsAttackingAnimKey, true);*/
		//pc.animator.SetTrigger(AttackTriggerAnimKey);
		/*pc.animator.SetFloat(AttackTypeAnimaKey, pc.curNode.animFloat);
		pc.curNode.Copy(pc.curNode);*/
		attackNode = pc.curNode;
		//effect = attackNode.effectPoolManager.ActiveObject(attackNode.effectPos.position, pc.transform.rotation);
		currentTime = 0;
		//cam.SetVibration(attackNode.shakeTime, attackNode.curveShakePower, attackNode.randomShakePower);

		pc.SetCollider(true);
		pc.attackCollider.radiusCollider.enabled = true;
		pc.attackCollider.SetCollider(attackNode.attackAngle, attackNode.attackLength/100);
	}

	public override void Update(PlayerController pc)
	{
		if(currentTime > attackNode.attackSpeed)
		{
			pc.ChangeState(PlayerState.AttackAfterDelay);
		}
		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		pc.rigid.velocity = Vector3.zero;

		pc.attackCollider.radiusCollider.enabled = false;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		FDebug.Log("1|" + other);
		if (other.CompareTag(unit.EnemyTag))
		{
			FDebug.Log("2|" + other);
			if (unit.attackCollider.IsInCollider(other.gameObject))
			{
				FDebug.Log("3|" + other);
				unit.playerData.Attack(other.GetComponent<UnitBase>());
			}
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}

	public override void OnCollisionStay(PlayerController unit, Collision collision)
	{

	}
}
