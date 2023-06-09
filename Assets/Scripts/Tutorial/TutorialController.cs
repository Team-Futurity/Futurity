using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// 튜토리얼 시나리오를 관리하고 제어하는 클래스입니다.
/// </summary>
public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private int stageNum; // 현재 진행 중인 튜토리얼 단계를 나타내는 변수입니다.

	[SerializeField]
	private QuestUIController questUIController; // 퀘스트 UI를 제어하는 컨트롤러입니다.

	[SerializeField]
	private List<CharacterDialogWindowOpener> dialogWindowOpeners; // dialogWindowOpener 리스트입니다.

	[SerializeField]
	private List<Quest> tutorialQuests; // tutorialQuest 리스트입니다.

	/// <summary>
	/// 스크립트가 시작될 때 호출되는 메서드입니다.
	/// 여기서는 대화창을 열고, 모든 퀘스트를 UI 컨트롤러에 추가하며, 퀘스트 완료 시 UI 업데이트 이벤트를 등록합니다.
	/// </summary>
	private void Start()
	{
		dialogWindowOpeners[1].CharacterDialogWindowOpen();

		foreach (Quest quest in tutorialQuests)
		{
			questUIController.AddQuest(quest);
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // 퀘스트 완료 시 UI를 업데이트합니다.
		}
	}

	/// <summary>
	/// 매 프레임마다 호출되는 메서드입니다.
	/// 여기서는 퀘스트의 조건을 체크하고, 모든 퀘스트가 완료되었는지 확인합니다.
	/// 모든 퀘스트가 완료되었다면, 튜토리얼 단계를 증가시키고 퀘스트 조건을 재설정합니다.
	/// </summary>
	private void Update()
	{
		CheckQuestConditions();

		if (AllQuestsCompleted())
		{
			stageNum++;
			ResetQuestConditions();
		}
	}

	/// <summary>
	/// 퀘스트 조건을 확인하고 해당 키가 눌렸는지 확인합니다.
	/// 조건이 충족되면 해당 퀘스트를 완료합니다.
	/// </summary>
	private void CheckQuestConditions()
	{
		foreach (Quest quest in tutorialQuests)
		{
			if (Input.GetKeyDown(quest.requiredKey))
			{
				quest.CompleteQuest();
			}
		}
	}

	/// <summary>
	/// 모든 퀘스트의 조건을 재설정합니다.
	/// </summary>
	public void ResetQuestConditions()
	{
		foreach (Quest quest in tutorialQuests)
		{
			quest.ResetQuest();
		}
	}

	/// <summary>
	/// 모든 퀘스트가 완료되었는지 확인합니다.
	/// </summary>
	/// <returns>모든 퀘스트가 완료되었다면 true를 반환하고, 그렇지 않다면 false를 반환합니다.</returns>
	private bool AllQuestsCompleted()
	{
		foreach (Quest quest in tutorialQuests)
		{
			if (!quest.IsCompleted)
			{
				return false;
			}
		}

		return true;
	}
}

/// <summary>
/// 퀘스트를 정의하는 클래스입니다.
/// 퀘스트 이름, 필요한 키 입력, 완료 이벤트, 완료 상태를 가집니다.
/// </summary>
[System.Serializable]
public class Quest
{
	public string questName; // 퀘스트 이름입니다.
	public string requiredKey; // 퀘스트를 완료하기 위해 필요한 키 코드입니다.
	public UnityEvent<Quest> OnQuestComplete = new UnityEvent<Quest>(); // 퀘스트가 완료될 때 호출되는 이벤트입니다.
	public bool IsCompleted { get; private set; } // 퀘스트의 완료 상태입니다.


	/// <summary>
	/// 퀘스트를 완료합니다.
	/// 이 메서드를 호출하면 완료 상태가 true가 되고, 완료 이벤트가 발생합니다.
	/// </summary>
	public void CompleteQuest()
	{
		IsCompleted = true;
		OnQuestComplete.Invoke(this);
	}

	/// <summary>
	/// 퀘스트를 재설정합니다.
	/// 이 메서드를 호출하면 완료 상태가 false로 변경됩니다.
	/// </summary>
	public void ResetQuest()
	{
		IsCompleted = false;
	}
}