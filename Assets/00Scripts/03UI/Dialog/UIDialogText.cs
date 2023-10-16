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

	private bool isRunning = false;
	
	public TMP_Text dialogText;
	private WaitForSeconds typingPrintSpeed;

	[HideInInspector]
	public UnityEvent OnEnd;

	private void Awake()
	{
		TryGetComponent(out dialogText);
		
		typingPrintSpeed = new WaitForSeconds(0.1f);
		isRunning = false;
	}

	public void Show(string text)
	{
		if (isRunning)
		{
			return;
		}

		ClearText();

		copyText = text;
		isRunning = true;
		
		if (usedTypingAnimation)
		{
			StartTyping();
			return;
		}
	}

	public void Restart()
	{
		if (!isRunning)
		{
			return;
		}
		
		StartTyping();
	}
	
	public void Stop()
	{
		if (!isRunning)
		{
			return;
		}
		
		StopCoroutine("StartTypingAnimation");
	}

	public void ClearText()
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
		
		// End
		ResetData();
	}

	private void ResetData()
	{
		copyText = "";
		isRunning = false;
		currentAccessIndex = 0;

		OnEnd?.Invoke();
	}
}
