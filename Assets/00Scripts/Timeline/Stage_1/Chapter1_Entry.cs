using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Chapter1_Entry : CutSceneBase
{
	[Header("컴포넌트")]
	[SerializeField] private PlayerCameraEffect cameraEffect;
	[SerializeField] private Transform[] playerMoveTarget;
	[SerializeField] private GameObject mapLight;
	[SerializeField] private float intensity;
	
	private TimelineManager manager;
	private PlayerController playerController;
	private Vignette vignette;
	
	protected override void Init()
	{
		manager = TimelineManager.Instance;
		
		vignette = cameraEffect.Vignette;
		intensity = vignette.intensity.value;
	}

	protected override void EnableCutScene()
	{
		mapLight.SetActive(false);
		manager.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
		
	}

	private void Update()
	{
		vignette.intensity.value = intensity;
	}

	public void MovePlayer()
	{
		manager.PlayerController.transform.position = playerMoveTarget[0].position;
		manager.PlayerController.LerpToWorldPosition(playerMoveTarget[1].position, 2.0f);
	}
}
