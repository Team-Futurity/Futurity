using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChargeCollisionData
{
	[Tooltip("�浹�� ���̾�")]				public LayerMask wallLayer;
	[Tooltip("�浹�ؼ� ���� �Ÿ�")]			public float collisionDistance;
	[Tooltip("ī�޶� ����ŷ ���� �ð�")]	public float shakeDuration;
	[Tooltip("ī�޶� ����ŷ �ӵ�")]			public float shakeVelocity;
	[Tooltip("����Ʈ�� ��µ� �Ÿ�")]		public float effectDistance;
	[Tooltip("����Ʈ ��ġ ������")]			public Vector3 effectPosOffset;
	[Tooltip("����Ʈ ȸ�� ������")]			public Vector3 effectRotOffset;
}

public class ActiveEffectToWall : MonoBehaviour
{
	[SerializeField] private ChargeCollisionData collisionData;
	private DamageInfo damageInfo;
	private ObjectPoolManager<Transform> poolManager;
	private Rigidbody rigidbody;
	private PlayerCamera playerCamera;
	private bool isOnEffect;

	public void RunCollision( ChargeCollisionData collisionData, DamageInfo info, ObjectPoolManager<Transform> poolManager, Rigidbody rb, PlayerCamera cameraData)
	{
		this.collisionData = collisionData;
		this.poolManager = poolManager;
		damageInfo = info;
		rigidbody = rb;
		playerCamera = cameraData;
		isOnEffect = false;
		enabled = true;
	}

	private void FixedUpdate()
	{
		if(poolManager == null) { enabled = false; }

		RaycastHit hit;

		if(!isOnEffect && Physics.Raycast(transform.position, rigidbody.velocity.normalized, out hit, collisionData.effectDistance, collisionData.wallLayer))
		{
			var effect = poolManager.ActiveObject(hit.point + collisionData.effectPosOffset + hit.normal * 0.01f, Quaternion.Euler(collisionData.effectRotOffset));
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
			enabled = false;
		}
	}
}
