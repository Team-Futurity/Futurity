using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class C1_A1_EntryCutScene : CutSceneBase
{
	[Space(6)] 
	[Header("=========== 추가 컴포넌트===========")]
	[SerializeField] private SpawnerManager spawnerManager;

	[Header("Sound")] 
	[SerializeField] private List<EventReference> soundList;
	private EventInstance soundInst;
	private int curIndex = 0;

	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		chapterManager.isCutScenePlay = true;
		chapterManager.SetActiveMainUI(false);
	}
	
	protected override void DisableCutScene()
	{
		chapterManager.SetActiveMainUI(true);
		chapterManager.isCutScenePlay = false;
		
		chapterManager.ResetGlitch();
	}

	public void Area1_StartSkeletonCutScene()
	{
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue, PlaySound);
	}
	
	public void Area1_Scripting()
	{
		base.StartScripting();
	}
	
	public void RandingPlayer()
	{
		chapterManager.PlayerController.PlayLandingAnimation();
	}

	public void Area1_SpawnEnemy()
	{
		spawnerManager.SpawnEnemy();
	}

	public void SetLandingAnimation()
	{
		chapterManager.PlayerController.SetLandingAnimation();
	}

	public void StopCutSceneSound()
	{
		soundInst.stop(STOP_MODE.IMMEDIATE);
		curIndex = 0;
	}

	private void PlaySound()
	{
		StopCutSceneSound();
		
		if (curIndex >= soundList.Count)
		{
			return;
		}
		
		soundInst = AudioManager.Instance.CreateInstance(soundList[curIndex++]);
		soundInst.start();
	}
}
