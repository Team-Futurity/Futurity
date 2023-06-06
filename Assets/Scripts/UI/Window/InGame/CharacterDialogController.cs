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

	/// <summary>
	/// ����� �ؽ�Ʈ�� �����մϴ�.
	/// </summary>
	/// <param name="texts">����� �ؽ�Ʈ ����Ʈ</param>
	public void SetTexts(List<string> texts)
	{
		fullText = texts;
	    textNum = 0;
	}

	/// <summary>
	/// CharacterDialogâ�� ��ȭâ�� �ؽ�Ʈ�� ����ϴ� ��ũ��Ʈ��.
	/// ����� �ؽ�Ʈ�� �ϳ��� ����ϰų�, ��� ���� �ؽ�Ʈ�� ��ŵ�մϴ�.
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

			//�ӽ� : ���� ���� ����, ũ��ƽ ���带 ����
			for(int i = 0; i < InGameUnitManager.Instance.enemys.Count; i++)
			{
				InGameUnitManager.Instance.enemys[i].gameObject.SetActive(true);
			}
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
	/// �ؽ�Ʈ Ÿ������ �Ϸ��ϰ� ���� ���¸� �����մϴ�.
	/// </summary>
	public void TypingTextEnd()
	{
		FDebug.Log($"{gameObject}�� \"{fullText}\" Text Ÿ���� �Ϸ�");
		textNum++;
		isTextEnd = true;
		showTextCoroutine = null;
	}

	/// <summary>
	/// ���� ��� ���� �ؽ�Ʈ�� ��ȯ�մϴ�.
	/// </summary>
	/// <returns>���� ������� �ؽ�Ʈ</returns>
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
	/// �ؽ�Ʈ ��� �Ϸ� ���θ� ��ȯ�մϴ�.
	/// </summary>
	/// <returns>�ؽ�Ʈ ��� �Ϸ� ����</returns>
	public bool GetTextEnd()
	{
		return isTextEnd;
	}

	/// <summary>
	/// ĳ���� ��������Ʈ�� �����մϴ�.
	/// </summary>
	/// <param name="changeSprite"></param>
	public void SetCharactorSprite(Sprite changeSprite)
	{
		charactorImage.sprite = changeSprite;
	}

	/// <summary>
	/// Ÿ���� ������ �ð��� �����մϴ�.
	/// </summary>
	/// <param name="delayTime">Ÿ���� ������ �ð�</param>
	public void SetTypingDelay(float delayTime)
	{
		typingDelay = delayTime;
	}
}
