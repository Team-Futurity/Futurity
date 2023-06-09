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
	private int textNum;

	[SerializeField]
	private List<string> test1;
	[SerializeField]
	private List<string> test2;
	[SerializeField]
	private List<string> test3;

	public void TextWriteButton()
	{
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




}
