using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class KeyBinding : MonoBehaviour
{
	//#주의#	InputActionReference를 inspecter창에서 할당해주기 때문에 사용중인 InputActionReference가 변경 혹은 삭제될 경우 문제가 발생할 수 있습니다.
	public InputActionReference actionReference;
	public TextMeshProUGUI buttonText;
	[SerializeField]
	private Button button;
	private bool waitingForKeyInput;

	private void Awake()
	{
		button.onClick.AddListener(StartKeyRemap);
		UpdateButtonText();
		KeyBindingManager.Instance.RegisterKeyBinding(this);
	}

	//#설명#	현재 버튼에 할당된 키를 표시하는 텍스트를 업데이트합니다.
	private void UpdateButtonText()
	{
		string currentBinding = actionReference.action.GetBindingDisplayString();
		buttonText.text = currentBinding;
	}

	//#설명#	사용자가 키를 변경하려 할 때 호출되는 함수입니다. 사용자 입력을 기다리고 새 키를 할당합니다.
	private void StartKeyRemap()
	{
		if (!waitingForKeyInput)
		{
			waitingForKeyInput = true;
			buttonText.text = "...";
			actionReference.action.PerformInteractiveRebinding()
				.WithControlsExcluding("Mouse")
				.OnMatchWaitForAnother(0.1f)
				.OnComplete(operation => KeyRemapFinished())
				.Start();
		}
	}

	//#설명#	키 변경 프로세스가 완료되면 호출되는 함수입니다. 텍스트를 업데이트하고 대기 상태를 종료합니다.
	private void KeyRemapFinished()
	{
		waitingForKeyInput = false;
		UpdateButtonText();
	}
}
