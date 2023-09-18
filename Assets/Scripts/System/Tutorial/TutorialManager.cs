using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TutorialManager : MonoBehaviour
{
	[SerializeField] private float fadeTime = 1f;

	[SerializeField] private UIPerformBoardHandler performHandler;

	[SerializeField] private UIDialogController dialogController;

	private IControlCommand dialogCommand;
	private IControlCommand perfromCommand;

	private List<DialogData> tutorialDialogList = new List<DialogData>();
	private readonly string dialogPath = "Assets/Data/Dialog/Tutorial/";
	private readonly string[] dialogKey =
	{
		"TutorialData1",
		"TutorialData2",
		"TutorialData3",
		"TutorialData4"
	};

	private int currentIndex = 0;

	private void Awake()
	{
		dialogController.TryGetComponent(out dialogCommand);
		performHandler.TryGetComponent(out dialogCommand);

		InputActionManager.Instance.DisableAllInputActionAsset();
		InputActionManager.Instance.EnableInputActionAsset(InputActionType.Player);

		LoadTutorialDialogData();
	}

	private void Start()
	{
		FadeManager.Instance.FadeOut(fadeTime, () =>
		{
			StartTutorial();
		});
	}

	private void StartTutorial()
	{
		// 처음 나타날 Dialog 세팅
		dialogController.SetDialogData(tutorialDialogList[currentIndex]);
		NextDialogData();

		// Perform Board 초기 세팅
		performHandler.SetPerfrom();

		dialogController.PlayDialog();

		// Dialog가 종료되면 Perform 시작하기
		dialogController.OnEnded.AddListener(() => {
			WindowManager.Instance.ShowWindow("Perform");
			performHandler.Run();
		});

		// Perform 종료 시, Next Dialog 출력
		performHandler.OnChangePerformBoard.AddListener(() =>
	   {
		   dialogController.SetDialogData(tutorialDialogList[currentIndex]);
		   NextDialogData();

		   dialogController.PlayDialog();

		   // 초기 세팅 지워주기
		   dialogController.OnEnded.RemoveAllListeners();

		   dialogController.OnEnded.AddListener(() =>
		  {
			  if (currentIndex >= 4)
			  {
				  FadeManager.Instance.FadeIn(fadeTime, () =>
				  {
					  SceneLoader.Instance.LoadScene("Chapter1-Stage1");
				  });
			  }
			  else
			  {
				  performHandler.ChangeToNextBoard();
			  }
		  });
	   });
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

	private void NextDialogData()
	{
		if(currentIndex < 4)
		{
			currentIndex++;
		}
	}
}