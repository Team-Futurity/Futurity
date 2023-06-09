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
	[Header ("Character�� �̹����� �����ϰų� �ؽ�Ʈ ����� �����ϴ� ��ũ��Ʈ")]
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
			FDebug.LogWarning($"{gameObject.name}�� CharacterDialogController�� playerInput�� �Ҵ����ּ���");
		}
		playerInput.enabled = false;

		if (currentText == "")
		{
			WriteCharactorText();
		}
	}

	/// <summary>
	/// ����� �ؽ�Ʈ�� �����մϴ�.
	/// </summary>
	public void SetTexts(List<string> texts)
	{
		fullText = texts;
	    textNum = 0;
	}

	/// <summary>
	/// CharacterDialogâ�� ��ȭâ�� �ؽ�Ʈ�� ����ϴ� ��ũ��Ʈ��. fullText�� ����Ǿ��ִ� �ؽ�Ʈ�� �������� ����ϸ�, ������� �ؽ�Ʈ�� �ִٸ� �ش� �ؽ�Ʈ�� ��ŵ�մϴ�.
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
	/// �ؽ�Ʈ�� �ϳ��� ����ϴ� �ڷ�ƾ�Դϴ�.
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
	/// ���� �ؽ�Ʈ�� �Ѿ�ϴ�.
	/// </summary>
	public void SkipNextText()
	{
		charactorText.text = fullText[textNum];
		TypingTextEnd();
	}

	/// <summary>
	/// �ؽ�Ʈ Ÿ���� �Ϸ�
	/// </summary>
	private void TypingTextEnd()
	{
		FDebug.Log($"{gameObject}�� \"{fullText}\" Text Ÿ���� �Ϸ�");
		textNum++;
		isTextEnd = true;
		showTextCoroutine = null;
	}

	/// <summary>
	/// ���� ������� �ؽ�Ʈ�� �����ɴϴ�.
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
	/// �ؽ�Ʈ ��� �Ϸ� ���θ� �����ɴϴ�.
	/// </summary>
	public bool GetTextEnd()
	{
		return isTextEnd;
	}


	/// <summary>
	/// Ÿ���� ������ �ð��� �����մϴ�.
	/// </summary>
	public void SetTypingDelay(float delayTime)
	{
		typingDelay = delayTime;
	}
}
