using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
	private PlayerController pc;
	[HideInInspector] public Transform effect;
	private AttackNode attackNode;

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

	public void ChargedEffectPooling(int number)
	{
		pc.rushObjectPool = new ObjectPoolManager<Transform>(pc.rushEffects[number].effect);
		var effect = pc.rushObjectPool.ActiveObject(pc.rushEffects[number].effectPos.position);
		var particles = effect.GetComponent<ParticleController>();
		particles.Initialize(pc.rushObjectPool);
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
