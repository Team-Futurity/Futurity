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

	//#����#	�� �Լ��� Ű �ؽ�Ʈ�� ó������ ������Ʈ�ϰ� ǥ���մϴ�. �� �ؽ�Ʈ�� �ش� ��ư�� ���� � Ű�� ���ε��Ǿ� �ִ����� �����ݴϴ�.
	private void UpdateButtonText()
	{
		string currentBinding = actionReference.action.GetBindingDisplayString();
		buttonText.text = currentBinding;
	}

	//#����#	�� �޼���� ����ڰ� Ű ���ε� ��ư�� ������� �۵��Ǹ�, Player�� Ű �Է��� ��ٸ��ϴ�.
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

	//#����#	KeyBindingManager�� Ű ���ε� ������ ���� �����ϴ� �Լ��Դϴ�.
	private void KeyRemapFinished()
	{
		waitingForKeyInput = false;
		newBinding = actionReference.action.GetBindingDisplayString();
		buttonText.text = newBinding;
		keyBindingManager.RegisterPendingBinding(actionReference, newBinding);
	}
}
