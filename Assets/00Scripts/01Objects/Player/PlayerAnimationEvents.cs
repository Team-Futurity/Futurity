using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
	private PlayerController pc;
	[SerializeField] private PlayerCameraEffect cameraEffect;

	[HideInInspector] public Transform effect;
	private AttackNode attackNode;

	private EffectActivationTime effectType;
	private EffectTarget EffectTarget;
	private Transform effectPos;
	private IEnumerator hitStop;

	public FMODUnity.EventReference walk;


	private void Start()
	{
		pc = GetComponent<PlayerController>();
	}

	public void EffectPooling()
	{
		attackNode = pc.curNode;

		if(attackNode == null ) { FDebug.LogError("[PlayerAnimationEvents] attackNode is Null. Please Check to Animation Event."); return; }
		if(attackNode.effectPoolManager == null ) { FDebug.LogError("[PlayerAnimationEvents] attackNode.effectPoolManager is Null. Please Check to Command Graph or Script"); return; }


		Quaternion playerRot = pc.gameObject.transform.localRotation;
		if(playerRot.y > 180f) { playerRot.y -= 360f; }
		Quaternion rotation = playerRot * attackNode.effectRotOffset;

		Vector3 position = pc.gameObject.transform.position + /*rotation */ attackNode.effectOffset;

		FDebug.Log($"{position} = {pc.gameObject.transform.position} + {rotation * attackNode.effectOffset}");


		position.y = pc.gameObject.transform.position.y + attackNode.effectOffset.y;

		effect = attackNode.effectPoolManager.ActiveObject(position, rotation);
		var particles = effect.GetComponent<ParticleController>();

		if(particles != null )
		{
			particles.Initialize(attackNode.effectPoolManager);
		}
	}

	public void PreAllocatedEffectPooling()
	{
		//pc.rushEffectManager.ActiveEffect(effectType, EffectTarget, effectPos);
	}

	public void AllocateEffect(EffectActivationTime activationTime, EffectTarget target, Transform effectPos)
	{
		this.effectPos = effectPos;
		effectType = activationTime;
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

	// 0 : Charge
	// 1 : Expolosion
	// 2 : Landing
	public void BasicActivePartAttackProc(int attackProccessOrder)
	{
		UnitState<PlayerController> GettedState = null;
		PlayerBasicPartState chargeState;
		if (!pc.GetState(PlayerState.BasicSM, ref GettedState)) { return; }

		chargeState = GettedState as PlayerBasicPartState;

		switch (attackProccessOrder)
		{
			case 0:
				chargeState.Charging();
				break;
			case 1:
				chargeState.Attack();
				break;
			case 2:
				chargeState.Landing();
				break;
			case 3:
				chargeState.AttackEnd();
				break;
			case 4:
				chargeState.PreAttack();
				break;
		}
	}

	public void CameraShake(string str)
	{
		if (cameraEffect == null)
		{
			return;
		}

		// 0 : velocity, 1 : Duration
		float[] value = ConvertStringToFloatArray(str);

		// attackNode = pc.curNode;
		cameraEffect.CameraShake(value[0], value[1]);
	}

	#region HitEffectEvent
	public void StartHitStop(float duration)
	{
		hitStop = HitStop(duration);
		StartCoroutine(hitStop);
	}
	public void SlowMotion(string value)
	{
		UnitState<PlayerController> state = null;
		pc.GetState(PlayerState.AttackDelay, ref state);
		int count = 0;

		if (state != null)
		{
			count = ((PlayerAttackBeforeDelayState)state).GetTargetCount();
		}

		if (count <= 0)
		{
			return;
		}

		float[] values = ConvertStringToFloatArray(value);
		cameraEffect.StartTimeScaleTimer(values[0], values[1]);
		cameraEffect.CameraShake();
	}
	
	private float[] ConvertStringToFloatArray(string input)
	{
		string[] strResult = input.Split(',');
		float[] result = new float[2];

		for (int i = 0; i < result.Length; ++i)
		{
			float.TryParse(strResult[i], out result[i]);
		}

		return result;
	}
	
	private IEnumerator HitStop(float duration)
	{
		Time.timeScale = 0.0f;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = 1.0f;
	}
	#endregion

	public void WalkSE()
	{
		AudioManager.instance.PlayOneShot(walk, transform.position);
	}

	public void SetCollider(int isActiveInteager)
	{
		bool isActive = isActiveInteager == 1;

		pc.SetCollider(isActive);
	}
}
