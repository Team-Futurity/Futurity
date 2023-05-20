using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

[FSMState((int)PlayerController.PlayerState.AttackDelay)]
public class PlayerAttackBeforeDelayState : UnitState<PlayerController>
{
	// Animation Key
	protected readonly string AttackTriggerAnimKey = "AttackTrigger";

	// etc
	private float currentTime;
	private Transform effect;
	//private CameraController cam;
	protected AttackNode attackNode;
	private List<GameObject> targets = new List<GameObject>();

	public override void Begin(PlayerController pc)
	{
		//pc.rigid.velocity = Vector3.zero;

		/*if(cam == null)	
			cam = Camera.main.GetComponent<CameraController>();*/

		string key = pc.currentAttackState == PlayerState.NormalAttack ? pc.ComboAttackAnimaKey : pc.ChargedAttackAnimaKey;


		if(!pc.animator.GetBool(pc.IsAttackingAnimKey))
		{
			pc.animator.SetTrigger(AttackTriggerAnimKey);
		}

		pc.animator.SetBool(pc.IsAttackingAnimKey, true);
		pc.animator.SetInteger(key, pc.curNode.animInteger);
		
		pc.curNode.Copy(pc.curNode);
		attackNode = pc.curNode;
		currentTime = 0;

		pc.glove.SetActive(true);

		// autoTargetting
		pc.autoTargetCollider.radiusCollider.enabled = true;
		pc.autoTargetCollider.SetCollider(360, attackNode.attackLength / 100);
		targets.Clear();
	}

	public override void Update(PlayerController pc)
	{
		if(targets.Count > 0)
		{
			AutoTarget.Instance.TurnToNearstObject(targets, pc.gameObject);
		}

		if(currentTime >= attackNode.attackDelay)
		{
			pc.ChangeState(pc.currentAttackState);
		}
		currentTime += Time.deltaTime;
	}

	public override void FixedUpdate(PlayerController unit)
	{
	}

	public override void End(PlayerController pc)
	{
		pc.autoTargetCollider.radiusCollider.enabled = false;
	}

	public override void OnTriggerEnter(PlayerController unit, Collider other)
	{
		if(other.CompareTag(unit.EnemyTag))
		{
			targets.Add(other.gameObject);
		}
	}

	public override void OnCollisionEnter(PlayerController unit, Collision collision)
	{

	}
}
