using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerAnimationEvents : MonoBehaviour
{
	private PlayerController pc;

	[HideInInspector] public Transform effect;
	private AttackNode attackNode;

	private EffectActivationTime effectType;
	private EffectTarget EffectTarget;
	private Transform effectPos;
	private IEnumerator hitStopCamShake;
	private IEnumerator hitStopNonShake;

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

		Quaternion rotation = Quaternion.identity;
		Vector3 position = Vector3.zero;
		AsyncOperationHandle<GameObject> effectObject = new AsyncOperationHandle<GameObject>();

		if (attackNode.effectParentType == EffectParent.World)
		{
			Quaternion playerRot = pc.gameObject.transform.rotation;
			if (playerRot.y > 180f) { playerRot.y -= 360f; }
			rotation = playerRot * attackNode.effectRotOffset;
			//FDebug.Log($"Player Rotation : {playerRot.eulerAngles}\nEffect Rotation Offset : {attackNode.effectRotOffset.eulerAngles}\nResult : {rotation.eulerAngles}");

			position = pc.gameObject.transform.position + rotation * attackNode.effectOffset;
			position.y = pc.gameObject.transform.position.y + attackNode.effectOffset.y;
			effect = attackNode.effectPoolManager.ActiveObject(position, rotation);
		}
		else
		{
			Quaternion playerRot = pc.gameObject.transform.localRotation;
			if (playerRot.y > 180f) { playerRot.y -= 360f; }
			rotation = attackNode.effectRotOffset;
			//FDebug.Log($"Player Rotation : {playerRot.eulerAngles}\nEffect Rotation Offset : {attackNode.effectRotOffset.eulerAngles}\nResult : {rotation.eulerAngles}");

			position = pc.gameObject.transform.localPosition + attackNode.effectOffset;
			position.y = pc.gameObject.transform.localPosition.y + attackNode.effectOffset.y;
			effect = attackNode.effectPoolManager.ActiveObject(attackNode.effectOffset, attackNode.effectRotOffset, false);
		}


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
	

	#region HitEffectEvent
	// 카메라 쉐이크
	public void CameraShake(string str)
	{
		// 0 : velocity, 1 : Duration
		float[] value = ConvertStringToFloatArray(str);

		// attackNode = pc.curNode;
		pc.cameraEffect.CameraShake(value[0], value[1]);
	}
	
	// 플레이어 피격에 대한 HitStop
	public void StartHitStop(float duration)
	{
		hitStopNonShake = HitStop(duration);
		StartCoroutine(hitStopNonShake);
	}
	
	// 플레이어 타격에 대한 HitStop
	public void HitStopCamShake(string values)
	{
		if (CheckEnemyInAttackRange() == false)
		{
			return;
		}
		
		float[] value = ConvertStringToFloatArray(values);
		hitStopCamShake = HitStopWithCamShake(value[0], value[1], value[2]);
		StartCoroutine(hitStopCamShake);
	}

	public void HitStopNonShake(float duration)
	{
		hitStopNonShake = HitStop(duration);
		StartCoroutine(hitStopNonShake);
	}
	
	private IEnumerator HitStopWithCamShake(float hitStopTime, float velocity, float duration)
	{
		Time.timeScale = 0.0f;
		yield return new WaitForSecondsRealtime(hitStopTime);
		
		Time.timeScale = 1.0f;
		pc.cameraEffect.CameraShake(velocity, duration);
	}

	private IEnumerator HitStop(float duration)
	{
		Time.timeScale = 0.0f;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = 1.0f;
	}
	
	private float[] ConvertStringToFloatArray(string input)
	{
		string[] strResult = input.Split(',');
		float[] result = new float[strResult.Length];

		for (int i = 0; i < result.Length; ++i)
		{
			float.TryParse(strResult[i], out result[i]);
		}

		return result;
	}

	private bool CheckEnemyInAttackRange()
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
			return false;
		}

		return true;
	}
	#endregion

	#region SetActiveScene
	public void StartAlphaCutScene()
	{
		TimelineManager.Instance.EnableActiveCutScene(EActiveCutScene.ACITVE_ALPHA);
	}
	#endregion
	public void EnableAttackTiming()
	{
		pc.playerData.EnableAttackTiming();
	}

	public void WalkSE()
	{
		AudioManager.Instance.PlayOneShot(walk, transform.position);
	}

	public void SetCollider(int isActiveInteager)
	{
		bool isActive = isActiveInteager == 1;

		pc.SetCollider(isActive);
	}
}
