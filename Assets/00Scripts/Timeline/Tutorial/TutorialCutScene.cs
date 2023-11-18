using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class TutorialCutScene : CutSceneBase
{
	[Header("추가 컴포넌트")]
	public UnityEvent onPauseEvent;

	[Header("사운드")] 
	[SerializeField] private EventReference tutorialBGM;
	private EventInstance soundInst;
	
	protected override void Init()
	{
		base.Init();
		soundInst = AudioManager.Instance.CreateInstance(tutorialBGM);
	}

	protected override void EnableCutScene()
	{
		
	}
	
	protected override void DisableCutScene()
	{
		FadeManager.Instance.FadeIn(0.8f, () => SceneLoader.Instance.LoadScene("Chapter1-Stage1", true));
	}
	
	public void Tutorial_Scripting()
	{
		StartScripting();
	}

	public void Tutorial_StartSkeletonCutScene()
	{
		soundInst.start();
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
	}

	public void EnableUIInput()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
	}
		
	public void Pause()
	{
		cutScene.Pause();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		
		onPauseEvent?.Invoke();
	}

	public void Resume()
	{
		cutScene.Resume();
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
	}

	public void StopCutSceneSound()
	{
		soundInst.stop(STOP_MODE.IMMEDIATE);
	}
}
