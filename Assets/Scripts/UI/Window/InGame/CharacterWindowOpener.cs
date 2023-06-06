using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterWindowOpener : MonoBehaviour
{
	[Header("게임 시작 시 캐릭터 대화창을 열고, 설정된 텍스트를 출력하는 클래스")]
	[Space(20)]

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
		if (canvas is not null)
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
