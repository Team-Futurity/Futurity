using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIController : MonoBehaviour
{
	[SerializeField]
	List<TextMeshProUGUI> questTextMeshPro = new List<TextMeshProUGUI>(); // 각 퀘스트의 텍스트를 출력하는 TextMeshProUGUI 객체의 리스트입니다.

	private List<Quest> questList = new List<Quest>(); // 관리하는 퀘스트의 리스트입니다.

	void Start()
	{
		ResetQuestText();
	}

	/// <summary>
	/// 새로운 퀘스트를 리스트에 추가하는 메서드입니다.
	/// </summary>
	/// <param name="newQuest">추가할 퀘스트입니다.</param>
	public void AddQuest(Quest newQuest)
	{
		questList.Add(newQuest);
		ResetQuestText();
	}

	/// <summary>
	/// 특정 퀘스트를 클리어 상태로 설정하는 메서드입니다.
	/// </summary>
	/// <param name="clearQuestNum">클리어할 퀘스트의 번호입니다.</param>
	public void StrikeThroughQuest(int clearQuestNum)
	{
		questTextMeshPro[clearQuestNum].fontStyle = FontStyles.Strikethrough;
	}

	/// <summary>
	/// 모든 퀘스트의 텍스트를 업데이트하는 메서드입니다.
	/// </summary>Y
	private void ResetQuestText()
	{
		for (int i = 0; i < questList.Count; i++)
		{
			questTextMeshPro[i].text = $"• {questList[i].questName}" + (questList[i].IsCompleted ? " (완료)" : "");
			questTextMeshPro[i].fontStyle = questList[i].IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;
		}
	}

	/// <summary>
	/// 퀘스트가 클리어되면 호출되는 메서드입니다.
	/// </summary>
	/// <param name="completedQuest">클리어된 퀘스트입니다.</param>
	public void UpdateQuestUI(Quest completedQuest)
	{
		int questIndex = questList.IndexOf(completedQuest);
		if (questIndex != -1)
		{
			ResetQuestText();
		}
	}

	/// <summary>
	/// 모든 퀘스트를 클리어 상태에서 일반 상태로 되돌리는 메서드입니다.
	/// </summary>
	public void ClearQuests()
	{
		foreach (Quest quest in questList)
		{
			quest.ResetQuest();
		}
		ResetQuestText();
	}
}
