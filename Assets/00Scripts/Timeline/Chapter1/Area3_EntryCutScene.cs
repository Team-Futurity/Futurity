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
		originIntensity = 0;
	}

	private void Update()
	{
		vignette.intensity.value = intensity;
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
	}

	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.scripting.EnableNameText((int)ScriptingStruct.ENameType.MIRAE);
		chapterManager.SetActiveMainUI(false);
		chapterManager.ChangeFollowTarget(true, playerMoveTarget);
		SetCameraVignette();
	}

	public override void DisableCutScene()
	{
		chapterManager.isCutScenePlay = false;
		chapterManager.SetActiveMainUI(true);
		spawnerManager.SpawnEnemy();

		vignette.intensity.value = originIntensity;
		vignette.color.value = Color.red;
		
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		//chapterManager.SetActivePlayerInput(true);
	}

	private void SetCameraVignette()
	{
		vignette.color.value = Color.black;
		vignette.intensity.value = intensity;
	}
	
	public void Area3_PrintScripts()
	{
		chapter1Director.Pause();

		chapterManager.PauseCutSceneUntilScriptsEnd(chapter1Director);
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
