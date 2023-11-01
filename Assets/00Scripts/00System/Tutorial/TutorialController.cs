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

	private void Awake()
	{
		InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
	}

	private void Start()
	{
		SetEvent();
		FadeManager.Instance.FadeOut(fadeTime);
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
		
		for (int i = 2; i < 8; ++i)
		{
			performHandler.AddPerformBoard(2, uiPerformBoards[i]);
		}
		
		for (int i = 8; i < 9; ++i)
		{
			performHandler.AddPerformBoard(3, uiPerformBoards[i]);
		}
	}
}