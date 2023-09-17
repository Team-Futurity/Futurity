using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TutorialManager : MonoBehaviour
{
	[SerializeField] private float fadeTime = 1f;

	[SerializeField] private UIPerformBoardHandler performHanlder;

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

	private void Awake()
	{
		dialogController.TryGetComponent(out dialogCommand);
		performHanlder.TryGetComponent(out dialogCommand);

		// InputActionManager.Instance.DisableAllInputActionAsset();
		// InputActionManager.Instance.EnableInputActionAsset(InputActionType.Player);

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
		//dialogController.SetDialogData(dialogData);
		//dialogController.PlayDialog();
	}

	private void LoadTutorialDialogData()
	{
		Debug.Log("CALL");
		foreach (var key in dialogKey)
		{
			DialogData dialogData = Addressables.LoadAsset<DialogData>(dialogPath + dialogKey +".assets").WaitForCompletion();
			tutorialDialogList.Add(dialogData);

			Debug.Log(dialogData.name);
		}
	}
}