using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TutorialController : MonoBehaviour
{
	[SerializeField] private float fadeTime = 1f;

	[SerializeField] private UIPerformBoardHandler performHandler;
	[SerializeField] private UIDialogController dialogController;

	
	// Dialog Data Set
	private List<DialogData> tutorialDialogList = new List<DialogData>();
	private readonly string dialogPath = "Assets/04Data/Dialog/Tutorial/";
	private readonly string[] dialogKey =
	{
		"TutorialData1",
		"TutorialData2",
		"TutorialData3"
	};

	// Only Debug
	[Space(10), Header("Debug ��� ����")]
	public bool isDebugMode = false;

	private void Awake()
	{
		if (!isDebugMode)
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		}

		LoadTutorialDialogData();
	}

	private void Start()
	{
		if (isDebugMode)
		{
			StartTutorial();
		}
		else
		{
			FadeManager.Instance.FadeOut(fadeTime, StartTutorial);
		}
	}

	private void StartTutorial()
	{
		//SceneLoader.Instance.LoadScene("Chapter1-Stage1");
		
		//// First Settings
		//dialogController.SetDialogData(tutorialDialogList[currentDialogIndex]);
		
		//// Event - Dialog Ended
		//dialogController.OnEnded?.AddListener(() =>
		//{
		//	switch (currentDialogIndex)
		//	{
		//		case 0:
		//			performHandler.SetPerfrom();
		//			performHandler.Run();
		//			break;

		//		case 1:
		//			performHandler.ChangeToNextBoard();
		//			break;
		//	}
		//});

		//// Event - Perform Changed
		//performHandler.OnChangePerformBoard?.AddListener(() =>
		//{
		//	switch (currentDialogIndex)
		//	{
		//		case 0:
		//			currentDialogIndex++;
		//			dialogController.SetDialogData(tutorialDialogList[currentDialogIndex]);
					
		//			dialogController.PlayDialog();
		//			break;
				
		//		case 1:
		//			performHandler.ChangeToNextBoard();
		//			currentDialogIndex++;
		//			break;
		//	}
		//});
		
		//// Event - Perform Ended
		//performHandler.OnEnded?.AddListener(() =>
		//{
		//	dialogController.SetDialogData(tutorialDialogList[maxDialogIndex]);
		//	dialogController.PlayDialog();
			
		//	dialogController.OnEnded?.RemoveAllListeners();
		//	dialogController.OnEnded?.AddListener(() =>
		//	{
		//		if (isDebugMode)
		//		{
		//			SceneLoader.Instance.LoadScene("Chapter1-Stage1");
		//		}
		//		else
		//		{
		//			FadeManager.Instance.FadeIn(fadeTime, () =>
		//			{
		//				SceneLoader.Instance.LoadScene("Chapter1-Stage1");
		//			});
		//		}
		//	});
		//});
		
		//// Play
		//dialogController.PlayDialog();
	}

	private void LoadTutorialDialogData()
	{
		foreach (var key in dialogKey)
		{
			var path = dialogPath + key + ".asset";

			DialogData dialogData = Addressables.LoadAsset<DialogData>(path).WaitForCompletion();
			tutorialDialogList.Add(dialogData);
		}
	}
}