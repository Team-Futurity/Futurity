using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIDialogText : MonoBehaviour
{
	[SerializeField]
	private bool usedTypingAnimation = false;

	private int currentAccessIndex = 0;
	private string copyText;

	public bool isRunning = false;

	public TMP_Text dialogText;
	private WaitForSeconds typingPrintSpeed;

	[HideInInspector]
	public UnityEvent onEnded;

	private void Awake()
	{
		TryGetComponent(out dialogText);

		typingPrintSpeed = new WaitForSeconds(0.1f);
		isRunning = false;
	}

	public void Show(string text)
	{
		// �������� ���, Return
		if (isRunning) { return; }

		// �ʱ�ȭ ���ְ�
		ClearText();

		// �ؽ�Ʈ ����
		copyText = text;
		isRunning = true;

		if (usedTypingAnimation)
		{
			StartTyping();
		}
	}

	public void Pass()
	{
		if (!isRunning) { return; }

		StopCoroutine("StartTypingAnimation");
		dialogText.text = copyText;

		ResetData();
	}

	private void ClearText()
	{
		dialogText.text = "";
	}

	private void StartTyping()
	{
		StartCoroutine("StartTypingAnimation");
	}

	private IEnumerator StartTypingAnimation()
	{
		for (; currentAccessIndex < copyText.Length; ++currentAccessIndex)
		{
			dialogText.text += copyText[currentAccessIndex].ToString();

			yield return typingPrintSpeed;
		}

		ResetData();
	}

	private void ResetData()
	{
		copyText = "";
		isRunning = false;
		currentAccessIndex = 0;

		onEnded?.Invoke();
	}
}
