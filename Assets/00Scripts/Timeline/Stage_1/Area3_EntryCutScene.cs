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
	[SerializeField] private Transform startPos;

	[Header("텍스트 출력 리스트")]
	[SerializeField] private List<ScriptingList> scriptsList;
	private int curScriptsIndex = 0;

	[Header("Only Use Timeline")] 
	[SerializeField] private float intensity;
	[SerializeField] private float scanLineJitter;
	[SerializeField] private float colorDrift;
	
	private TimelineManager manager;
	private PlayerController playerController;
	private Vignette vignette;
	private float originIntensity;

	protected override void Init()
	{
		manager = TimelineManager.Instance;
		
		vignette = cameraEffect.Vignette;
		originIntensity = vignette.intensity.value;
	}

	protected override void EnableCutScene()
	{
		manager.uiCanvas.SetActive(false);
		manager.SetActivePlayerInput(false);
		manager.ChangeFollowTarget(true, playerMoveTarget);

		vignette.intensity.value = intensity;
		vignette.color.value = Color.black;
		
		manager.PlayerController.transform.position = startPos.transform.position;
		manager.PlayerController.transform.rotation = Quaternion.identity;
		
		Invoke(nameof(PlayCutScene), 0.3f);
	}

	public override void DisableCutScene()
	{
		manager.SetActivePlayerInput(true);
		manager.uiCanvas.SetActive(true);
		spawnerManager.SpawnEnemy();

		vignette.intensity.value = originIntensity;
		vignette.color.value = Color.red;
	}
	
	private void Update()
	{
		vignette.intensity.value = intensity;
		manager.AnalogGlitch.scanLineJitter.value = scanLineJitter;
		manager.AnalogGlitch.colorDrift.value = colorDrift;
	}

	public void Area3_PrintScripts()
	{
		chapter1Director.Pause();

		StartCoroutine(PrintScripts());
		manager.scripting.StartPrintingScript(scriptsList[curScriptsIndex].scriptList);
	}

	private IEnumerator PrintScripts()
	{
		while (manager.scripting.isEnd == false)
		{
			yield return null;
		}
		
		chapter1Director.Resume();
		manager.scripting.isEnd = false;

		if (curScriptsIndex + 1 < scriptsList.Count)
		{
			curScriptsIndex++;
			yield return new WaitForSecondsRealtime(0.2f);
			manager.scripting.InitNameField(scriptsList[curScriptsIndex].scriptList[0].name);
		}
		else
		{
			curScriptsIndex = 0;
		}
	}
	
	public void MovePlayer()
	{
		manager.PlayerController.LerpToWorldPosition(playerMoveTarget.position, 1.5f);
	}

	private void PlayCutScene()
	{
		gameObject.GetComponent<PlayableDirector>().Play();
	}
}
