using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class Area3_EntryCutScene : CutSceneBase
{
	[Header("컴포넌트")] 
	[SerializeField] private PlayableDirector chapter1Director;
	[SerializeField] private PlayerCameraEffect cameraEffect;
	[SerializeField] private Transform playerMoveTarget;
	[SerializeField] private SpawnerManager spawnerManager;

	[Header("텍스트 출력 리스트")]
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;

	[Header("Only Use Timeline")] 
	[SerializeField] private float intensity;
	
	private PlayerController playerController;
	private Vignette vignette;
	private float originIntensity;

	protected override void Init()
	{
		vignette = cameraEffect.Vignette;

		originIntensity = vignette.intensity.value;
	}

	private void Update()
	{
		vignette.intensity.value = intensity;
	}

	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.SetActiveMainUI(false);
		chapterManager.SetActivePlayerInput(false);
		chapterManager.ChangeFollowTarget(true, playerMoveTarget);

		vignette.intensity.value = intensity;
		vignette.color.value = Color.black;
		
		Invoke(nameof(PlayCutScene), 0.3f);
	}

	public override void DisableCutScene()
	{
		chapterManager.isCutScenePlay = false;
		chapterManager.SetActivePlayerInput(true);
		chapterManager.SetActiveMainUI(true);
		spawnerManager.SpawnEnemy();

		vignette.intensity.value = originIntensity;
		vignette.color.value = Color.red;
	}
	
	public void Area3_PrintScripts()
	{
		chapter1Director.Pause();

		chapterManager.PauseCutSceneUntilScriptsEnd(chapter1Director, scriptsList, curScriptsIndex);
		chapterManager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
	
		curScriptsIndex = (curScriptsIndex + 1 < scriptsList.Count) ? curScriptsIndex + 1 : 0;
	}
	
	public void MovePlayer()
	{
		chapterManager.PlayerController.LerpToWorldPosition(playerMoveTarget.position, 1.5f);
	}

	private void PlayCutScene()
	{
		gameObject.GetComponent<PlayableDirector>().Play();
	}
}
