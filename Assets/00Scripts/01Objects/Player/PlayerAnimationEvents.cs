using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private ObjectPoolManager<Transform> walkEffectPoolManager;

	public FMODUnity.EventReference walk;
	public GameObject worldEffectParent;
	public GameObject walkEffectPrefab;
	public Transform walkEffectTransform;
	public bool isSlowMotionEnable = false;

	private EventInstance currentVoice;

	private void Start()
	{
		pc = transform.parent.GetComponent<PlayerController>();
		walkEffectPoolManager = new ObjectPoolManager<Transform>(walkEffectPrefab, worldEffectParent);
	}

	public void EffectPooling()
	{
		attackNode = pc.curNode;
		AttackAsset attackAsset = attackNode.GetAttackAsset(pc.partSystem.GetEquiped75PercentPointPartCode());

		if(attackNode == null ) { FDebug.LogError("[PlayerAnimationEvents] attackNode is Null. Please Check to Animation Event."); return; }
		if(attackAsset == null || attackAsset.effectPoolManager == null ) { FDebug.LogError("[PlayerAnimationEvents] attackAsset or attackAsset.effectPoolManager is Null. Please Check to Command Graph or Script"); return; }

		Quaternion rotation = Quaternion.identity;
		Vector3 position = Vector3.zero;

		if (attackAsset.effectParentType == EffectParent.Local)
		{
			Quaternion playerRot = pc.gameObject.transform.localRotation;
			if (playerRot.y > 180f) { playerRot.y -= 360f; }
			rotation = attackAsset.effectRotOffset;
			//FDebug.Log($"Player Rotation : {playerRot.eulerAngles}\nEffect Rotation Offset : {attackNode.effectRotOffset.eulerAngles}\nResult : {rotation.eulerAngles}");

			position = pc.gameObject.transform.localPosition + attackAsset.effectOffset;
			position.y = pc.gameObject.transform.localPosition.y + attackAsset.effectOffset.y;
			effect = attackAsset.effectPoolManager.ActiveObject(attackAsset.effectOffset, attackAsset.effectRotOffset, false);
		}
		else
		{
			Quaternion playerRot = pc.gameObject.transform.rotation;
			if (playerRot.y > 180f) { playerRot.y -= 360f; }
			rotation = playerRot * attackAsset.effectRotOffset;
			//FDebug.Log($"Player Rotation : {playerRot.eulerAngles}\nEffect Rotation Offset : {attackNode.effectRotOffset.eulerAngles}\nResult : {rotation.eulerAngles}");

			position = pc.gameObject.transform.position + rotation * attackAsset.effectOffset;
			position.y = pc.gameObject.transform.position.y + attackAsset.effectOffset.y;
			effect = attackAsset.effectPoolManager.ActiveObject(position, rotation);
		}


		var particles = effect.GetComponent<ParticleController>();

		if(particles != null )
		{
			particles.Initialize(attackAsset.effectPoolManager);
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
				//chargeState.UpAttack();
				break;
			case 1:
				//chargeState.DownAttack();
				break;
			case 2:
				//chargeState.EnemyLanding();
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
 
	public void BetaActivePartAttackProc(int attackProcessOrder)
	{
		UnitState<PlayerController> GettedState = null;
		PlayerBetaPartState betaState;
		if (!pc.GetState(PlayerState.BetaSM, ref GettedState)) { return; }
		
		betaState = GettedState as PlayerBetaPartState;

		switch (attackProcessOrder)
		{
			// Phase 1 - Start
			case 1:
				betaState.OnFirstPhase();
				break;
			
			// Phase 1 - End
			case 2:
				betaState.OnFirstPhaseEnded();
				break;
			
			// Phase 2 - Start
			case 3:
				betaState.OnSecondPhase();
				break;
			
			// Phase 2 - End
			case 4:
				betaState.OnSecondPhaseEnded();
				break;
			
			// Phase 3 - Start
			case 5:
				betaState.OnThirdPhase();
				break;
			
			// Phase 3 - End
			case 6:
				betaState.OnThirdPhaseEnded();
				break;
			
			// Active End
			case 7:
				betaState.OnEnded();
				break;
			
		}
		
	}
	
	public void WalkEffect()
	{
		if(walkEffectPoolManager == null) { return; }
		GameObject obj = walkEffectPoolManager.ActiveObject(walkEffectTransform.position, walkEffectTransform.rotation).gameObject;

		var particle = obj.GetComponent<ParticleController>();
		if(particle != null)
		{
			particle.Initialize(walkEffectPoolManager);
		}
	}

	#region HitEffectEvent
	// 카메라 쉐이크
	public void CameraShake(string str)
	{
		// 0 : velocity, 1 : Duration
		float[] value = ConvertStringToFloatArray(str);

		// attackNode = pc.curNode;
		if(pc.camera != null)
		{
			pc.camera.CameraShake(value[0], value[1]);
		}
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

	public void PlayerAttackSlowMotion(int index)
	{
		if (CheckEnemyInAttackRange() == false || pc.comboGaugeSystem.CurrentGauge < 100)
		{
			return;
		}

		if (pc.camera != null)
		{
			pc.camera.TimeScaleManager.StartAttackSlowMotion(index);
		}
	}

	public void StartChromaticAberration(string values)
	{
		if (CheckEnemyInAttackRange() == false)
		{
			return;
		}
		
		float[] value = ConvertStringToFloatArray(values);

		if(pc.camera != null)
		{
			pc.camera.StartChromaticAberration(value[0], value[1]);
		}
	}
	
	private IEnumerator HitStopWithCamShake(float hitStopTime, float velocity, float duration)
	{
		Time.timeScale = 0.0f;
		yield return new WaitForSecondsRealtime(hitStopTime);
		
		Time.timeScale = 1.0f;

		if (pc.camera != null)
		{
			pc.camera.CameraShake(velocity, duration);
		}
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
		//TimelineManager.Instance.EnableActiveCutScene(EActiveCutScene.ACITVE_ALPHA);
	}
	#endregion
	
	public void EnableAttackTiming()
	{
		pc.playerData.EnableAttackTiming();
	}

	// 0 : frameCount
	// 1 : skipFrameCount
	// 2 : animation speed
	// 3 : blend time
	public void AlterAnimationSpeed(string data)
	{
		if (!CheckEnemyInAttackRange()) { return; }

		float[] result = ConvertStringToFloatArray(data);
		pc.playerData.AlterAnimationSpeed((int)result[0], (int)result[1], result[2], (int)result[3]);
	}

	public void WalkSE()
	{
		AudioManager.Instance.PlayOneShot(walk, transform.position);
	}

	public void OutputAttackVoice()
	{
		var voice = pc.curNode.attackVoice;

		if (voice.IsNull) { return; }

		if(currentVoice.isValid())
		{
			currentVoice.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		currentVoice = AudioManager.Instance.CreateInstance(voice);
		currentVoice.set3DAttributes(RuntimeUtils.To3DAttributes(pc.gameObject));
		currentVoice.start();
	}

	private const string ALPHA = "ALPHA";
	private const string BETA = "BETA";
	
	public void EnableActiveCutScene(string type)
	{
		ECutSceneType cutSceneType = ECutSceneType.NONE;
		
		if (Equals(type, ALPHA) == true)
		{
			cutSceneType = ECutSceneType.ACTIVE_ALPHA;
		}
		else if (Equals(type, BETA) == true)
		{
			cutSceneType = ECutSceneType.ACTIVE_BETA;
		}
		
		TimelineManager.Instance.EnableCutScene(cutSceneType);
	}

	#region Rumble
	public void RumbleCurrentAttackNode()
	{
		if (!CheckEnemyInAttackRange()) { return; }

		RumbleManager.Instance.RumblePulse(attackNode.rumbleLow, attackNode.rumbleHigh, attackNode.rumbleDuration);
	}

	public void RumbleCurrentAttackNode(string rumbleValues)
	{
		if (!CheckEnemyInAttackRange()) { return; }

		float[] value = ConvertStringToFloatArray(rumbleValues);

		RumbleManager.Instance.RumblePulse(value[0], value[1], value[2]);
	}
	#endregion

	#region Collider
	public void DisableCollider()
	{
		pc.SetCollider(false);
		pc.attackColliderChanger.LockColliderEnable();
		pc.autoTargetColliderChanger.LockColliderEnable();
	}

	public void EnableCollider()
	{
		pc.autoTargetColliderChanger.UnlockColliderEnable();
		pc.attackColliderChanger.UnlockColliderEnable();
		pc.SetCollider(true);
	}

	public void EnableAttackCollider(int type)
	{
		pc.attackColliderChanger.UnlockColliderEnable();
		pc.attackColliderChanger.EnableCollider((ColliderType)type);
	}

	public void SetCollider(int isActiveInteager)
	{
		bool isActive = isActiveInteager == 1;

		if(isActive)
		{
			pc.playerData.RestoreCollider();
			pc.attackColliderChanger.LockColliderEnable();
			pc.autoTargetColliderChanger.LockColliderEnable();
		}
		else
		{
			pc.playerData.DisableAllCollider();
			pc.attackColliderChanger.UnlockColliderEnable();
			pc.autoTargetColliderChanger.UnlockColliderEnable();
		}
	}
	#endregion
}
