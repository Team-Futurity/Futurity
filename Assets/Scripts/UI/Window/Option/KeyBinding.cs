using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class KeyBinding : MonoBehaviour
{
	[Header("��ݵ�� �Ҵ��")]
	public InputActionReference actionReference;
	[SerializeField]
	private KeyBindingManager keyBindingManager;
	[SerializeField]
	private Button button;
	[SerializeField]
	private TextMeshProUGUI buttonText;

	[Space(10)]
	[Header("������ ���ε��� ����ؾ��� ��� �Ҵ��")]
	[SerializeField]
	[Tooltip("�������� ����ϴ� Ű�� �Ҵ��� true �ƴҰ�� ���� false")]
	private bool isControlTypeVector3 = false;
	[SerializeField]
	[Tooltip ("�������� ����ϴ� Ű�� �Ҵ��Ҷ��� ���� �־��ּ��� �� �ܿ��� ����")]
	private string moveActionName;
	[SerializeField]
	int subBindingIndex;

	[Space(10)]
	private bool waitingForKeyInput;
	[SerializeField]
	private string newBinding;
	[SerializeField]
	private string currentBinding;

	private void Start()
	{
		button.onClick.AddListener(StartKeyRemap);
		UpdateButtonText();
	}

	//#����#	�� �Լ��� Ű �ؽ�Ʈ�� ó������ ������Ʈ�ϰ� ǥ���մϴ�. �� �ؽ�Ʈ�� �ش� ��ư�� ���� � Ű�� ���ε��Ǿ� �ִ����� �����ݴϴ�.
	private void UpdateButtonText()
	{
		if (newBinding != "")
		{
			if (!isControlTypeVector3)
			{
				currentBinding = actionReference.action.GetBindingDisplayString();
				buttonText.text = newBinding.Replace("<Keyboard>/", "").ToUpper();
			}
			else
			{
				subBindingIndex = actionReference.action.bindings.IndexOf(binding => binding.name == moveActionName);


				if (subBindingIndex != -1)
				{
					currentBinding = actionReference.action.bindings[subBindingIndex].path;
					buttonText.text = newBinding.Replace("<Keyboard>/", "").ToUpper();
				}
				else
				{
					Debug.Log($"moveActionName�� ���������� �ԷµǾ��ִ��� Ȯ���ϼ���. \n bindingIndex : {subBindingIndex}");
				}
			}
		}
		else
		{
			if (!isControlTypeVector3)
			{
				currentBinding = actionReference.action.GetBindingDisplayString();
				buttonText.text = currentBinding;
			}
			else
			{
				subBindingIndex = actionReference.action.bindings.IndexOf(binding => binding.name == moveActionName);

				currentBinding = actionReference.action.bindings[subBindingIndex].path;
				buttonText.text = currentBinding.Replace("<Keyboard>/", "").ToUpper();
			}
		}
	}

	public void RegisterPendingBinding(InputActionReference actionReference, string newBindingKey)
	{
		//#����# �ش� �Լ��� keyBindingManager Directory���� ���� �������ݴϴ�.
		keyBindingManager.RegisterPendingBinding(actionReference, newBindingKey);
	}



	//#����#	�� �޼���� ����ڰ� Ű ���ε� ��ư�� ������� �۵��Ǹ�, Player�� Ű �Է��� ��ٸ��µ��� text���� "..."���� �Ӵϴ�.
	private void StartKeyRemap()
	{
		if (!waitingForKeyInput)
		{
			waitingForKeyInput = true;
			buttonText.text = "...";

			StartCoroutine(WaitForKeyInput());
		}
	}

	private IEnumerator WaitForKeyInput()
	{
		while (!Keyboard.current.anyKey.wasPressedThisFrame)
		{
			yield return null;
		}

		foreach (var keyControl in Keyboard.current.allKeys)
		{
			if (keyControl.wasPressedThisFrame)
			{
				newBinding = "<Keyboard>/" + keyControl.name;
				break;
			}
		}

		if (!isControlTypeVector3)
		{
			keyBindingManager.RegisterPendingBinding(actionReference, newBinding);
		}
		else
		{
			keyBindingManager.RegisterMovePendingBinding(actionReference, newBinding, moveActionName);
		}

		waitingForKeyInput = false;
		UpdateButtonText();
	}



	/*
	private void StartKeyRemap()
	{
		if (!waitingForKeyInput)
		{
			if (!isControlTypeVector3)
			{
				waitingForKeyInput = true;
				buttonText.text = "...";

				//�÷��̾�� ���� �޾ƿ��� ��ũ��Ʈ �߰� �ۼ� �ʿ�

				keyBindingManager.RegisterPendingBinding(actionReference, newBinding);
				waitingForKeyInput = false;
			}
			else
			{
				waitingForKeyInput = true;
				buttonText.text = "...";

				//�÷��̾�� ���� �޾ƿ��� ��ũ��Ʈ �߰� �ۼ� �ʿ�

				keyBindingManager.RegisterMovePendingBinding(actionReference, newBinding, moveActionName);
				waitingForKeyInput = false;
			}
		}
	}
	*/
}
