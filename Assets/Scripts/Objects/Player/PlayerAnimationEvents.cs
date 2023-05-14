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
		effect = attackNode.effectPoolManager.ActiveObject(attackNode.effectPos.position, pc.transform.rotation);
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
