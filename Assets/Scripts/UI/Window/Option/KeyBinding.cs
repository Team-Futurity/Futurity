using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class KeyBinding : MonoBehaviour
{
	//#����#	InputActionReference�� inspecterâ���� �Ҵ����ֱ� ������ ������� InputActionReference�� ���� Ȥ�� ������ ��� ������ �߻��� �� �ֽ��ϴ�.
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

	//#����#	���� ��ư�� �Ҵ�� Ű�� ǥ���ϴ� �ؽ�Ʈ�� ������Ʈ�մϴ�.
	private void UpdateButtonText()
	{
		string currentBinding = actionReference.action.GetBindingDisplayString();
		buttonText.text = currentBinding;
	}

	//#����#	����ڰ� Ű�� �����Ϸ� �� �� ȣ��Ǵ� �Լ��Դϴ�. ����� �Է��� ��ٸ��� �� Ű�� �Ҵ��մϴ�.
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

	//#����#	Ű ���� ���μ����� �Ϸ�Ǹ� ȣ��Ǵ� �Լ��Դϴ�. �ؽ�Ʈ�� ������Ʈ�ϰ� ��� ���¸� �����մϴ�.
	private void KeyRemapFinished()
	{
		waitingForKeyInput = false;
		UpdateButtonText();
	}
}
