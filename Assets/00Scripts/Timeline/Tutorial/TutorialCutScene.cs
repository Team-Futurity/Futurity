using UnityEngine;
using UnityEngine.Events;

public class TutorialCutScene : CutSceneBase
{
	public UnityEvent onPauseEvent;
	
	protected override void Init()
	{
		base.Init();
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
		chapterManager.StartSkeletonCutScene(cutScene, skeletonQueue);
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
}
