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

	// Dialog Data Set
	private List<DialogData> tutorialDialogList = new List<DialogData>();

	// Only Debug
	[Space(10), Header("Debug Mode")]
	public bool isDebugMode = false;

	private void Awake()
	{
		if (!isDebugMode)
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.Player);
		}
		else
		{
			InputActionManager.Instance.ToggleActionMap(InputActionManager.Instance.InputActions.UIBehaviour);
		}
	}

	private void Start()
	{
	}
}

