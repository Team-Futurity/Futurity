using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraEffect : MonoBehaviour
{
	[Header("Shake Camera")] 
	[SerializeField] private CinemachineImpulseSource impulseSource;

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			CameraShake();
		}
	}

	public void CameraShake(float velocity = 0.4f, float duration = 0.2f)
	{
		impulseSource.m_ImpulseDefinition.m_ImpulseDuration = duration;
		impulseSource.GenerateImpulseWithForce(velocity);
	}
}
