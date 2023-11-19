using FMODUnity;
using UnityEngine;

public class BossDeathCutScene : CutSceneBase
{
	[Header("Sound")] 
	[SerializeField] private EventReference bgmSound;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		chapterManager.SetActiveMainUI(false);
	}

	protected override void DisableCutScene()
	{
		StopBackGroundMusic();
		SceneLoader.Instance.LoadScene("CreditScene", false);
	}

	public void BossDeath_StartSkeleton()
	{
		AudioManager.Instance.RunBackgroundMusic(bgmSound);
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}

	public void StopBackGroundMusic() => AudioManager.Instance.StopBackgroundMusic();
	
}
