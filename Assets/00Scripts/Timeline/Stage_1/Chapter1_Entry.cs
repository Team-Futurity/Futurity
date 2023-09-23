using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Chapter1_Entry : CutSceneBase
{
	[Header("컴포넌트")] 
	[SerializeField] private PlayableDirector chapter1Director;
	[SerializeField] private PlayerCameraEffect cameraEffect;
	[SerializeField] private Transform[] playerMoveTarget;
	[SerializeField] private GameObject mapLight;
	[SerializeField] private float intensity;

	[Header("텍스트 출력 리스트")] 
	[SerializeField] private List<string> textList;
	
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

	public void Stage1_PrintScripts()
	{
		chapter1Director.Pause();

		StartCoroutine(PrintScripts());
		manager.StartPrintingScript(textList);
	}

	private IEnumerator PrintScripts()
	{
		while (manager.isEnd == false)
		{
			yield return null;
		}
		
		chapter1Director.Resume();
	}
	
	public void MovePlayer()
	{
		manager.PlayerController.transform.position = playerMoveTarget[0].position;
		manager.PlayerController.LerpToWorldPosition(playerMoveTarget[1].position, 1.5f);
	}
}
