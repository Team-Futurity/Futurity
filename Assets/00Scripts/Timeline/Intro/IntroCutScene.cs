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

	private bool isInputCheck = false;
	private bool isPause = false;
	
	
	protected override void Init()
	{
		base.Init();
	}

	protected override void EnableCutScene()
	{
		FadeManager.Instance.FadeOut(fadeInTime);
		
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		InputActionManager.Instance.RegisterCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck);
	}

	public override void DisableCutScene()
	{
		InputActionManager.Instance.DisableActionMap();
		InputActionManager.Instance.RemoveCallback(InputActionManager.Instance.InputActions.UIBehaviour.ClickUI, InputCheck);
		
		Time.timeScale = 1.0f;
		FadeManager.Instance.FadeIn(fadeOutTime, () =>
		{
			SceneLoader.Instance.LoadScene(nextSceneName);
		});
	}
	
	private void Update()
	{
		if (isInputCheck == false)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.F) && isPause == true)
		{
			Time.timeScale = 1.0f;
			introCutScene.Resume();
			isPause = false;
		}
		else if (Input.GetKeyDown(KeyCode.F) && isPause == false)
		{
			SkipToNextImg();
		}
	}

	private void InputCheck(InputAction.CallbackContext context)
	{
		if (isPause == true)
		{
			Time.timeScale = 1.0f;
			introCutScene.Resume();
			isPause = false;
		}
		else 
		{
			SkipToNextImg();
		}
	}
	public void ToggleInputCheck() => isInputCheck = !isInputCheck;
	public void PauseTimeline()
	{
		introCutScene.Pause();
		isPause = true;
	}

	private void SkipToNextImg()
	{
		Time.timeScale = 3.0f;
	}
}
