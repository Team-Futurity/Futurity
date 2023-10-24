using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class IntroCutScene : CutSceneBase
{
	[Header("Component")] 
	[SerializeField] private PlayableDirector introCutScene;
	
	[Header("Fade Time")] 
	[SerializeField] private float fadeInTime = 0.8f;
	[SerializeField] private float fadeOutTime = 0.8f;

	[Header("다음으로 이동할 씬 이름")] 
	[SerializeField] private string nextSceneName;
	private bool isPause = false;
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		FadeManager.Instance.FadeOut(fadeInTime);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck, true);
	}

	protected override void DisableCutScene()
	{
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck);
		
		Time.timeScale = 1.0f;
		FadeManager.Instance.FadeIn(fadeOutTime, () =>
		{
			SceneLoader.Instance.LoadScene(nextSceneName);
		});
	}
	
	private void InputCheck(InputAction.CallbackContext context)
	{
		if (isPause == false)
		{
			return;
		}
		
		introCutScene.Resume();
		isPause = false;
	}
	
	public void PauseTimeline()
	{
		introCutScene.Pause();
		isPause = true;
	}
}
