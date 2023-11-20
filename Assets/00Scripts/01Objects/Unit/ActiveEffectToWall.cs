using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChargeCollisionData
{
	[Tooltip("충돌할 레이어")]				public LayerMask wallLayer;
	[Tooltip("충돌해서 멈출 거리")]			public float collisionDistance;
	[Tooltip("카메라 쉐이킹 지속 시간")]	public float shakeDuration;
	[Tooltip("카메라 쉐이킹 속도")]			public float shakeVelocity;
	[Tooltip("이펙트가 출력될 거리")]		public float effectDistance;
	[Tooltip("이펙트 위치 오프셋")]			public Vector3 effectPosOffset;
	[Tooltip("이펙트 회전 오프셋")]			public Vector3 effectRotOffset;
}

public class ActiveEffectToWall : MonoBehaviour
{
	[SerializeField] private ChargeCollisionData collisionData;
	private DamageInfo damageInfo;
	private ObjectPoolManager<Transform> poolManager;
	private Rigidbody rigidbody;
	private PlayerCamera playerCamera;
	private bool isOnEffect;
	private EventInstance collisionSound;

	public void RunCollision( ChargeCollisionData collisionData, DamageInfo info, ObjectPoolManager<Transform> poolManager, Rigidbody rb, PlayerCamera cameraData, EventInstance sound)
	{
		this.collisionData = collisionData;
		this.poolManager = poolManager;
		damageInfo = info;
		rigidbody = rb;
		playerCamera = cameraData;
		isOnEffect = false;
		enabled = true;
		collisionSound = sound;

	}

	private void FixedUpdate()
	{
		if(poolManager == null) { enabled = false; }

		RaycastHit hit;

		if(!isOnEffect && Physics.Raycast(transform.position, rigidbody.velocity.normalized, out hit, collisionData.effectDistance, collisionData.wallLayer))
		{
			var effect = poolManager.ActiveObject(hit.point + collisionData.effectPosOffset + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(collisionData.effectRotOffset));
			var particle = effect.GetComponent<ParticleController>();
			if(particle != null)
			{
				particle.Initialize(poolManager);
			}
			isOnEffect = true;
		}

		if(isOnEffect && Physics.Raycast(transform.position, rigidbody.velocity.normalized, out hit, collisionData.collisionDistance, collisionData.wallLayer))
		{
			rigidbody.velocity = Vector3.zero;
			damageInfo.Attacker.InstantAttack(damageInfo);
			playerCamera?.CameraShake(collisionData.shakeVelocity, collisionData.shakeDuration);
			collisionSound.start();
			enabled = false;
		}
	}
}
