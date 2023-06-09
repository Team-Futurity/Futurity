using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterDialogController : MonoBehaviour
{
	[Header ("Character의 이미지를 변경하거나 텍스트 출력을 변경하는 스크립트")]
	[Space (15)]

	[SerializeField]
	private WindowController windowController;

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

	[SerializeField]
	private UnityEngine.InputSystem.PlayerInput playerInput;

	public UnityEvent characterDialogEndEvent;

	private void Start()
	{
		if (playerInput == null)
		{
			playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityEngine.InputSystem.PlayerInput>();
			FDebug.LogWarning($"{gameObject.name}의 CharacterDialogController에 playerInput를 할당해주세요");
		}
		playerInput.enabled = false;

		if (currentText == "")
		{
			WriteCharactorText();
		}
	}

	/// <summary>
	/// 출력할 텍스트를 설정합니다.
	/// </summary>
	public void SetTexts(List<string> texts)
	{
		fullText = texts;
	    textNum = 0;
	}

	/// <summary>
	/// CharacterDialog창의 대화창의 텍스트를 출력하는 스크립트로. fullText에 저장되어있는 텍스트를 차근차근 출력하며, 출력중인 텍스트가 있다면 해당 텍스트를 스킵합니다.
	/// </summary>
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

	/// <summary>
	/// 텍스트를 하나씩 출력하는 코루틴입니다.
	/// </summary>
	IEnumerator ShowText()
	{
		if (fullText.Count > textNum)
		{
			fullText[textNum] = fullText[textNum].Replace("\\n", "\n");
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
			characterDialogEndEvent?.Invoke();
			playerInput.enabled = true;
			windowController.WindowClose();
		}
	}

	/// <summary>
	/// 다음 텍스트로 넘어갑니다.
	/// </summary>
	public void SkipNextText()
	{
		charactorText.text = fullText[textNum];
		TypingTextEnd();
	}

	/// <summary>
	/// 텍스트 타이핑 완료
	/// </summary>
	private void TypingTextEnd()
	{
		FDebug.Log($"{gameObject}의 \"{fullText}\" Text 타이핑 완료");
		textNum++;
		isTextEnd = true;
		showTextCoroutine = null;
	}

	/// <summary>
	/// 현재 출력중인 텍스트를 가져옵니다.
	/// </summary>
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

	/// <summary>
	/// 텍스트 출력 완료 여부를 가져옵니다.
	/// </summary>
	public bool GetTextEnd()
	{
		return isTextEnd;
	}


	/// <summary>
	/// 타이핑 딜레이 시간을 설정합니다.
	/// </summary>
	public void SetTypingDelay(float delayTime)
	{
		typingDelay = delayTime;
	}
}
