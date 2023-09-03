using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDialogText : MonoBehaviour
{
	[SerializeField] 
	private bool usedTypingAnimation = false;

	private int currentAccessIndex = 0;
	private string copyText;

	private bool isRunning = false;
	
	private TMP_Text dialogText;
	private WaitForSeconds typingPrintSpeed;

	private void Awake()
	{
		TryGetComponent(out dialogText);
		
		typingPrintSpeed = new WaitForSeconds(0.1f);
		isRunning = false;

		dialogText.text = "";
	}

	public void Show(string text)
	{
		if (isRunning)
		{
			return;
		}

		dialogText.text = "";
		copyText = text;
		isRunning = true;
		
		if (usedTypingAnimation)
		{
			StartTyping();
			
			return;
		}

		Pass();
	}

	public void Pass()
	{
		if (!isRunning)
		{
			return;
		}
		
		if (usedTypingAnimation)
		{
			Stop();
		}

		dialogText.text = copyText;
		
		ResetData();
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
	}
}
