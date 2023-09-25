using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class Chapter1_Entry : CutSceneBase
{
	[Header("컴포넌트")] 
	[SerializeField] private PlayableDirector chapter1Director;
	[SerializeField] private PlayerCameraEffect cameraEffect;
	[SerializeField] private Transform[] playerMoveTarget;
	[SerializeField] private SpawnerManager spawnerManager;

	[Header("텍스트 출력 리스트")]
	[SerializeField] private ScriptsStruct[] scriptsList;
	private int curScriptsIndex = 0;

	[Header("Only Use Timeline")] 
	[SerializeField] private float intensity;
	[SerializeField] private float scanLineJitter;
	[SerializeField] private float colorDrift;
	
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
		manager.uiCanvas.SetActive(false);
		manager.SetActivePlayerInput(false);
	}

	public override void DisableCutScene()
	{
	}

	private void OnDisable()
	{
		manager.SetActivePlayerInput(true);
		manager.uiCanvas.SetActive(true);
		spawnerManager.SpawnEnemy();
		vignette.color.value = Color.red;
		
		gameObject.SetActive(false);
	}

	private void Update()
	{
		vignette.intensity.value = intensity;
		manager.AnalogGlitch.scanLineJitter.value = scanLineJitter;
		manager.AnalogGlitch.colorDrift.value = colorDrift;
	}

	public void Stage1_PrintScripts()
	{
		chapter1Director.Pause();

		StartCoroutine(PrintScripts());
		manager.StartPrintingScript(scriptsList[curScriptsIndex++].scriptsList);
	}

	private IEnumerator PrintScripts()
	{
		while (manager.isEnd == false)
		{
			yield return null;
		}
		
		chapter1Director.Resume();
		manager.isEnd = false;
	}
	
	public void MovePlayer()
	{
		manager.PlayerController.transform.position = playerMoveTarget[0].position;
		manager.PlayerController.LerpToWorldPosition(playerMoveTarget[1].position, 1.5f);
	}
}

[System.Serializable]
public struct ScriptsStruct
{
	public List<string> scriptsList;
}
