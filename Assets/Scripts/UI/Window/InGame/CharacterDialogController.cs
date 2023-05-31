using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterDialogController : MonoBehaviour
{
	[Header ("Character�� �̹����� �����ϰų� �ؽ�Ʈ ����� �����ϴ� ��ũ��Ʈ")]
	[Space (15)]

	[SerializeField]
	private WindowController windowController;

	[SerializeField]
	private UnityEngine.UI.Image charactorImage;
	[SerializeField]
	private TextMeshProUGUI charactorText;

	[SerializeField]
	private float typingDelay = 0.05f;
	[SerializeField]
	private List<string> fullText;
	[SerializeField]
	private int textNum = 0;

	private string currentText = "";
	private bool isTextEnd = false;

	private Coroutine showTextCoroutine;


	public void SetTexts(List<string> texts)
	{
		fullText = texts;
	    textNum = 0;
	}

	public void WriteCharactorText()
	{
		if (showTextCoroutine != null)
		{
			StopCoroutine(showTextCoroutine);
			SkipNextText();
		}
		else
		{
			currentText = "";
			charactorText.text = "";
			isTextEnd = false;
			showTextCoroutine = StartCoroutine(ShowText());
		}
	}

	IEnumerator ShowText()
	{
		if (fullText.Count > textNum)
		{
			for (int i = 0; i <= fullText[textNum].Length; i++)
			{
				currentText = fullText[textNum].Substring(0, i);
				charactorText.text = currentText;
				yield return new WaitForSeconds(typingDelay);
			}
			TypingTextEnd();
		}
		else
		{
			windowController.WindowClose();
		}
	}

	public void SkipNextText()
	{
		charactorText.text = fullText[textNum];
		TypingTextEnd();
	}

	public void TypingTextEnd()
	{
		FDebug.Log($"{gameObject}�� \"{fullText}\" Text Ÿ���� �Ϸ�");
		textNum++;
		isTextEnd = true;
		showTextCoroutine = null;
	}

	public string GetThisText()
	{
		if (fullText.Count > textNum)
		{
			return fullText[textNum];
		} 
		else
		{
			return fullText[fullText.Count - 1];
		}
	}
	public bool GetTextEnd()
	{
		return isTextEnd;
	}

	public void SetCharactorSprite(Sprite changeSprite)
	{
		charactorImage.sprite = changeSprite;
	}
	public void SetTypingDelay(float delayTime)
	{
		typingDelay = delayTime;
	}
}
