using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterDialogWindowOpener : MonoBehaviour
{
	[SerializeField]
	private bool isStartOpen;

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private GameObject characterWindow;

	[SerializeField]
	private List<string> texts;

	private GameObject currentCharacterWindow;
	private CharacterDialogController characterDialogController;
	private WindowManager windowManager;

	private void Start()
	{
		windowManager = WindowManager.Instance;
		if (isStartOpen)
		{
			if (canvas != null)
			{
				currentCharacterWindow = windowManager.WindowOpen(characterWindow, canvas.transform, false, Vector2.zero, Vector2.one);
			}
			else
			{
				currentCharacterWindow = windowManager.WindowTopOpen(characterWindow, false, Vector2.zero, Vector2.one);
			}

			characterDialogController = currentCharacterWindow.GetComponent<CharacterDialogController>();

			characterDialogController.SetTexts(texts);
			characterDialogController.WriteCharactorText();
		}
	}

	public void CharacterDialogWindowOpen()
	{
		if (canvas != null)
		{
			currentCharacterWindow = windowManager.WindowOpen(characterWindow, canvas.transform, false, Vector2.zero, Vector2.one);
		}
		else
		{
			currentCharacterWindow = windowManager.WindowTopOpen(characterWindow, false, Vector2.zero, Vector2.one);
		}

		characterDialogController = currentCharacterWindow.GetComponent<CharacterDialogController>();

		characterDialogController.SetTexts(texts);
		characterDialogController.WriteCharactorText();
	}
}
