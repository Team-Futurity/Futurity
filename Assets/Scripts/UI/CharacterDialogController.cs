using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterDialogController : MonoBehaviour
{
	[SerializeField]
	private UnityEngine.UI.Image charactorImage;
	[SerializeField]
	private TextMeshProUGUI charactorText;

	[SerializeField]
	private float typingTelay = 0.05f;
	[SerializeField]
	private string fullText;
	private string currentText = "";
	private bool isTextEnd = false;


	public void SetCharactorText(string changeText)
	{
		fullText = changeText;
		currentText = "";
		charactorText.text = "";
		isTextEnd = false;
		StartCoroutine(ShowText());
	}

	IEnumerator ShowText()
	{
		for (int i = 0; i <= fullText.Length; i++)
		{
			currentText = fullText.Substring(0, i);
			charactorText.text = currentText;
			yield return new WaitForSeconds(typingTelay);
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



	public void SetCharactorSprite(Sprite changeSprite)
	{
		charactorImage.sprite = changeSprite;
	}
}
