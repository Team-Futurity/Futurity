using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class KeyBinding : MonoBehaviour
{
	public InputActionReference actionReference;
	[SerializeField]
	private KeyBindingManager keyBindingManager;
	[SerializeField]
	private Button button;
	[SerializeField]
	private TextMeshProUGUI buttonText;
	private bool waitingForKeyInput;
	private string newBinding;

	private void Start()
	{
		button.onClick.AddListener(StartKeyRemap);
		UpdateButtonText();
	}

	//#설명#	이 함수는 키 텍스트를 처음으로 업데이트하고 표시합니다. 이 텍스트는 해당 버튼이 현재 어떤 키에 바인딩되어 있는지를 보여줍니다.
	private void UpdateButtonText()
	{
		string currentBinding = actionReference.action.GetBindingDisplayString();
		buttonText.text = currentBinding;
	}

	//#설명#	이 메서드는 사용자가 키 바인딩 버튼을 누를경우 작동되며, Player의 키 입력을 기다립니다.
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

	//#설명#	KeyBindingManager에 키 바인딩 데이터 값을 전달하는 함수입니다.
	private void KeyRemapFinished()
	{
		waitingForKeyInput = false;
		newBinding = actionReference.action.GetBindingDisplayString();
		buttonText.text = newBinding;
		keyBindingManager.RegisterPendingBinding(actionReference, newBinding);
	}
}
