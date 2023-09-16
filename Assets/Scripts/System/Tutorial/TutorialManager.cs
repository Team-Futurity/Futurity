using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	[SerializeField] private float fadeTime = 1f;

	[SerializeField] private UIPerformBoardHandler performHanlder;

	[SerializeField] private UIDialogController dialogController;

	private IControlCommand dialogCommand;
	private IControlCommand perfromCommand;
	
	private void Awake()
	{
		dialogController.TryGetComponent(out dialogCommand);
		performHanlder.TryGetComponent(out dialogCommand);
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
		dialogController.SetDialogData("TEST");
		dialogController.ShowDialog();
		dialogController.PlayDialog();
		
		// Dialog End�� Perform Action �־��ְ�
		// Dialog�� ��� End�� Clear�� �Ǵ�����.
	}
	
}