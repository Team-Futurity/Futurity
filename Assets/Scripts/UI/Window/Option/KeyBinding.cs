using TMPro;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class KeyBinding : MonoBehaviour
{
	[Header("↓반드시 할당↓")]
	public InputActionReference actionReference;
	[SerializeField]
	private KeyBindingManager keyBindingManager;
	[SerializeField]
	private Button button;
	[SerializeField]
	private TextMeshProUGUI buttonText;

	[Space(10)]
	[Header("↓하위 바인딩을 사용해야할 경우 할당↓")]
	[SerializeField]
	[Tooltip("움직임을 담당하는 키를 할당경우 true 아닐경우 전부 false")]
	private bool isControlTypeVector3 = false;
	[SerializeField]
	[Tooltip ("움직임을 담당하는 키를 할당할때만 값을 넣어주세요 그 외에는 공백")]
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

	//#설명#	이 함수는 키 텍스트를 처음으로 업데이트하고 표시합니다. 이 텍스트는 해당 버튼이 현재 어떤 키에 바인딩되어 있는지를 보여줍니다.
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
					Debug.Log($"moveActionName이 정상적으로 입력되어있는지 확인하세요. \n bindingIndex : {subBindingIndex}");
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
		//#설명# 해당 함수는 keyBindingManager Directory에게 값을 전달해줍니다.
		keyBindingManager.RegisterPendingBinding(actionReference, newBindingKey);
	}



	//#설명#	이 메서드는 사용자가 키 바인딩 버튼을 누를경우 작동되며, Player의 키 입력을 기다리는동안 text값을 "..."으로 둡니다.
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

				//플레이어에게 값을 받아오는 스크립트 추가 작성 필요

				keyBindingManager.RegisterPendingBinding(actionReference, newBinding);
				waitingForKeyInput = false;
			}
			else
			{
				waitingForKeyInput = true;
				buttonText.text = "...";

				//플레이어에게 값을 받아오는 스크립트 추가 작성 필요

				keyBindingManager.RegisterMovePendingBinding(actionReference, newBinding, moveActionName);
				waitingForKeyInput = false;
			}
		}
	}
	*/
}
