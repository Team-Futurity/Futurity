using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private float fadeTime = 1f;

	[SerializeField]
	private UIPerformBoardHandler performHandler;

	public List<UIPerformBoard> uiPerformBoards;

	public TutorialCutScene cutScene;

	public ComboGaugeSystem combo;

	private void Awake()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		ClearPartData();
	}
	
	private void ClearPartData()
	{
		for (int i = 0; i < 3; ++i)
		{
			PlayerPrefs.SetInt($"PassivePart{i}", 0);
		}
		PlayerPrefs.SetInt("ActivePart", 0);
	}

	private void Start()
	{
		SetEvent();
		FadeManager.Instance.FadeOut(fadeTime);
		
		uiPerformBoards[6].onShow?.AddListener(() =>
		{
			combo.ChangeComboGauge(100);
		});
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F9))
		{
			SceneLoader.Instance.LoadScene(ChapterSceneName.CHAPTER1_1);
		}
	}

	private void SetEvent()
	{
		performHandler.CreateGroup(1);
		performHandler.CreateGroup(2);
		performHandler.CreateGroup(3);

		for (int i = 0; i < 2; ++i)
		{
			performHandler.AddPerformBoard(1, uiPerformBoards[i]);
		}
		
		for (int i = 2; i < 9; ++i)
		{
			performHandler.AddPerformBoard(2, uiPerformBoards[i]);
		}
		
		for (int i = 9; i < 10; ++i)
		{
			performHandler.AddPerformBoard(3, uiPerformBoards[i]);
		}

		int next = 1;
		cutScene.onPauseEvent?.AddListener(() =>
		{
			performHandler.OpenPerform(next);
			next++;
			
			performHandler.onEnded?.AddListener(cutScene.Resume);
		});
	}
}