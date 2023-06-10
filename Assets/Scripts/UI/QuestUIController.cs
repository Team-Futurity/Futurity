using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuestUIController : MonoBehaviour
{
	[SerializeField]
	List<TextMeshProUGUI> questTextMeshPro = new List<TextMeshProUGUI>(); // 각 퀘스트의 텍스트를 출력하는 TextMeshProUGUI 리스트입니다.

	[SerializeField]
	List<Image> questImages = new List<Image>(); // 이미지를 표시할 UI Image 리스트입니다.

	private List<Quest> questList = new List<Quest>(); // 해당 스크립트가 관리하는 퀘스트의 리스트입니다.

	private void Start()
	{
		ResetQuestText();
	}

	/// <summary>
	/// 새로운 퀘스트를 리스트에 추가합니다.
	/// </summary>
	/// <param name="newQuest">추가할 퀘스트입니다.</param>
	public void AddQuest(Quest newQuest)
	{
		questList.Add(newQuest);
		ResetQuestText();
	}

	/// <summary>
	/// 특정 퀘스트를 클리어 상태로 설정합니다.
	/// </summary>
	/// <param name="clearQuestNum">클리어할 퀘스트의 번호입니다.</param>
	public void StrikeThroughQuest(int clearQuestNum)
	{
		questTextMeshPro[clearQuestNum].fontStyle = FontStyles.Strikethrough;
	}

	/// <summary>
	/// 모든 퀘스트의 텍스트와 이미지를 업데이트합니다.
	/// </summary>
	private void ResetQuestText()
	{
		for (int i = 0; i < questList.Count; i++)
		{
			// 퀘스트 이미지가 있을 경우에만 이미지를 표시하고, 없는 경우 텍스트를 표시합니다.
			if (questList[i].questImage != null)
			{
				questImages[i].sprite = questList[i].questImage;
				questImages[i].enabled = true;
				questTextMeshPro[i].enabled = false;
			}
			else
			{
				questTextMeshPro[i].text = $"• {questList[i].questName}" + (questList[i].IsCompleted ? " (완료)" : "");
				questTextMeshPro[i].fontStyle = questList[i].IsCompleted ? FontStyles.Strikethrough : FontStyles.Normal;
				questImages[i].enabled = false;
				questTextMeshPro[i].enabled = true;
			}
		}
	}

	/// <summary>
	/// 퀘스트가 클리어되면 호출합니다.
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
	/// 모든 퀘스트를 초기화 하여 빈공간으로 만듭니다.
	/// </summary>
	public void ClearQuests()
	{
		questList.Clear();

		for (int i = 0; i < questTextMeshPro.Count; i++)
		{
			questTextMeshPro[i].text = $"•";
			questTextMeshPro[i].fontStyle = FontStyles.Normal;
		}

		ResetQuestText();
	}
}

