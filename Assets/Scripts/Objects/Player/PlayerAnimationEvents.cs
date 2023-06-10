using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
	private PlayerController pc;

	[HideInInspector] public Transform effect;
	private AttackNode attackNode;

	private EffectType effectType;
	private EffectTarget EffectTarget;
	private Transform effectPos;

	public FMODUnity.EventReference walk;


	private void Start()
	{
		pc = GetComponent<PlayerController>();
	}

	public void EffectPooling()
	{
		attackNode = pc.curNode;
		effect = attackNode.effectPoolManager.ActiveObject(attackNode.effectPos.position, attackNode.effectPos.rotation);
		var particles = effect.GetComponent<ParticleController>();
		particles.Initialize(attackNode.effectPoolManager);
	}

	public void PreAllocatedEffectPooling()
	{
		pc.rushEffectManager.ActiveEffect(effectType, EffectTarget, effectPos);
	}

	public void AllocateEffect(EffectType type, EffectTarget target, Transform effectPos)
	{
		this.effectPos = effectPos;
		effectType = type;
		EffectTarget = target;
	}

	// 0 : UpAttack
	// 1 : DownAttack
	// 2 : Landing
	public void RushAttackProc(int attackProccessOrder)
	{
		UnitState<PlayerController> GettedState = null;
		PlayerAttackState_Charged chargeState;
		if(!pc.GetState(PlayerState.ChargedAttack, ref GettedState)) { return; }

		chargeState = GettedState as PlayerAttackState_Charged;

		switch (attackProccessOrder)
		{
			case 0:
				chargeState.UpAttack();
				break;
			case 1:
				chargeState.DownAttack();
				break;
			case 2:
				chargeState.EnemyLanding();
				break;
		}
	}

	public void CameraShake()
	{
		CameraController cam;
		cam = Camera.main.GetComponent<CameraController>();
		cam.SetVibration(attackNode.shakeTime, attackNode.curveShakePower, attackNode.randomShakePower);
	}

	public void WalkSE()
	{
		AudioManager.instance.PlayOneShot(walk, transform.position);
	}

	public void MoveWithAttack(float pow)
	{
		pc.rigid.velocity = pc.transform.forward * pow;
	}
}
