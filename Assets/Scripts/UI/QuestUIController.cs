using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIController : MonoBehaviour
{
	[SerializeField]
	List<TextMeshProUGUI> questTextMeshPro = new List<TextMeshProUGUI>();
	[SerializeField]
	List<string> questListText = new List<string>();


	// Start is called before the first frame update
	void Start()
    {
		ResetQuestText();
	}

	public void SetQuest(string newQuestText, int setQuestNum)
	{
		questListText[setQuestNum] = newQuestText;
		questTextMeshPro[setQuestNum].fontStyle = FontStyles.Normal;
		ResetQuestText();
	}

	public void SetQuestList(List<string> newQuestList)
	{
		questListText = newQuestList;
		ResetQuestText();
		for (int i = 0; i < 5; i++)
		{
			questTextMeshPro[i].fontStyle = FontStyles.Normal;
		}
	}

	public void ClearQuest(int clearQuestNum)
	{
		questTextMeshPro[clearQuestNum].fontStyle = FontStyles.Strikethrough;
	}

	private void ResetQuestText()
	{
		for (int i = 0; i < 5; i++)
		{
			questTextMeshPro[i].text = $"• {questListText[i]}";
		}
	}
}
