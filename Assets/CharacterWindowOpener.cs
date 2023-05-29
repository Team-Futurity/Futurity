using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterWindowOpener : MonoBehaviour
{
	[SerializeField]
	Canvas canvas;

	[SerializeField]
	GameObject characterWindow;

	[SerializeField]
	private List<string> texts;

	GameObject currentCharacterWindow;
	CharacterDialogController characterDialogController;

	private void Start()
	{
		if (canvas != null)
		{
			currentCharacterWindow = WindowManager.Instance.WindowOpen(characterWindow, canvas.transform, false, Vector2.zero, Vector2.one);
		}
		else
		{
			currentCharacterWindow = WindowManager.Instance.WindowTopOpen(characterWindow, false, Vector2.zero, Vector2.one);
		}

		characterDialogController = currentCharacterWindow.GetComponent<CharacterDialogController>();

		characterDialogController.SetTexts(texts);
		characterDialogController.WriteCharactorText();
	}
}
