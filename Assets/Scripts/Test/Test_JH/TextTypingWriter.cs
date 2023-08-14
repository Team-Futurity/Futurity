using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTypingWriter : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI typingText;

	[SerializeField]
	private float typingDelay = 0.05f;
	[SerializeField]
	private string fullText;
	private string currentText = "";
	private bool isTextEnd = false;

	private void Start()
	{
		SetText(fullText);
	}

	public void SetText(string changeText)
	{
		fullText = changeText;
		currentText = "";
		typingText.text = "";
		isTextEnd = false;
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		for (int i = 0; i <= fullText.Length; i++)
		{
			currentText = fullText.Substring(0, i);
			typingText.text = currentText;
			yield return new WaitForSeconds(typingDelay);
		}
		TypingTextEnd();
	}

	public void TypingTextEnd()
	{
		Debug.Log($"{gameObject}의 \"{fullText}\" Text 타이핑 완료");
		isTextEnd = true;
	}

	public string GetThisText()
	{
		return fullText;
	}
	public bool GetTextEnd()
	{
		return isTextEnd;
	}
}
