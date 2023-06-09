using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TutorialCharacterDialogTextWriter : MonoBehaviour
{
	[SerializeField]
	private CharacterDialogController characterDialogController;
	[SerializeField]
	private CharacterDialogWindowOpener characterDialogWindowOpener;

	[SerializeField]
	private int textNum;

	[SerializeField]
	private List<string> test1;
	[SerializeField]
	private List<string> test2;
	[SerializeField]
	private List<string> test3;

	private void Start()
	{
		FindCharacterDialogWindow();
	}

	public void TextWriteButton()
	{
		FindCharacterDialogWindow();

		switch (textNum)
		{
			case 0:
				characterDialogController.SetTexts(test1);
				textNum++;
				break;

			case 1:
				characterDialogController.SetTexts(test2);
				textNum++;
				break;

			case 2:
				characterDialogController.SetTexts(test3);
				textNum++;
				break;

			default:
				break;
		}
	}
	public void FindCharacterDialogWindow()
	{
		characterDialogController = WindowManager.Instance.FindWindow($"CharacterDialogWindow(Clone)").GetComponent<CharacterDialogController>();

		if (characterDialogController == null)
		{
			characterDialogWindowOpener.CharacterDialogWindowOpen();
			characterDialogController = WindowManager.Instance.FindWindow($"CharacterDialogWindow(Clone)").GetComponent<CharacterDialogController>();
		}
	}



}
