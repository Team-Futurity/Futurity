using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// 튜토리얼 시나리오를 관리하고 제어하는 클래스입니다.
/// </summary>
public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private int stageNum = 1; // 현재 진행 중인 튜토리얼 단계를 나타내는 변수입니다.

	[SerializeField]
	private QuestUIController questUIController; // 퀘스트 UI를 제어하는 컨트롤러입니다.

	[SerializeField]
	private UnityEngine.InputSystem.PlayerInput playerInput;
	[SerializeField]
	private PlayerController playerController;
	private AttackNode attackNode;

	[SerializeField]
	private List<CharacterDialogWindowOpener> dialogWindowOpeners; // dialogWindowOpener 리스트입니다.

	[SerializeField]
	private StageEndPotalController warpPotalController;

	[SerializeField]
	private UnityEvent tutorialEndEvent; // 튜토리얼 마지막 단계를 지난 이후 출력하는 이벤트입니다.

	[SerializeField]
	private List<Quest> activeQuests; // 현재 활성화된 퀘스트 리스트입니다.

	[SerializeField]
	private Vector3 questImagePos; // quest 이미지 위치입니다.
	[SerializeField]
	private List<Quest> tutorialQuests1; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Sprite> tutorialQuestSprites1; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Sprite> tutorialQuestClearSprites1; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Quest> tutorialQuests2; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Sprite> tutorialQuestSprites2; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Sprite> tutorialQuestClearSprites2; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Quest> tutorialQuests3; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Sprite> tutorialQuestSprites3; // tutorialQuest 리스트입니다.
	[SerializeField]
	private List<Sprite> tutorialQuestClearSprites3; // tutorialQuest 리스트입니다.


	/// <summary>
	/// 스크립트가 시작될 때 호출되는 메서드입니다.
	/// 여기서는 대화창을 열고, 모든 퀘스트를 UI 컨트롤러에 추가하며, 퀘스트 완료 시 UI 업데이트 이벤트를 등록합니다.
	/// </summary>
	private void Start()
	{
		QuestWriter();

		stageNum = 0;
		activeQuests = tutorialQuests1; // 처음에는 첫 번째 퀘스트 리스트를 활성화합니다.

		dialogWindowOpeners[0].CharacterDialogWindowOpen();

		for (int i = 0; i < tutorialQuests1.Count; i++)
		{
			tutorialQuests1[i].questImage = tutorialQuestSprites1[i];
			tutorialQuests1[i].questClearImage = tutorialQuestClearSprites1[i];
			tutorialQuests1[i].questPos = questImagePos;
		}
		for (int i = 0; i < tutorialQuests2.Count; i++)
		{
			tutorialQuests2[i].questImage = tutorialQuestSprites2[i];
			tutorialQuests2[i].questClearImage = tutorialQuestClearSprites2[i];
			tutorialQuests2[i].questPos = questImagePos;
		}
		for (int i = 0; i < tutorialQuests3.Count; i++)
		{
			tutorialQuests3[i].questImage = tutorialQuestSprites3[i];
			tutorialQuests3[i].questClearImage = tutorialQuestClearSprites3[i];
			tutorialQuests3[i].questPos = questImagePos;
		}



		foreach (Quest quest in tutorialQuests1)
		{
			questUIController.AddQuest(quest);
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // 퀘스트 완료 시 UI를 업데이트합니다.
		}
	}


	/// <summary>
	/// 퀘스트의 조건을 체크하고, 모든 퀘스트가 완료되었는지 확인합니다.
	/// 퀘스트가 완료되었다면, 튜토리얼 단계를 증가시키고 퀘스트 조건을 재설정합니다.
	/// </summary>
	private void Update()
	{
		attackNode = playerController.curNode;

		CheckQuestConditions(activeQuests);

		if (AllQuestsCompleted(activeQuests))
		{
			activeQuests.Clear();

			stageNum++;
			switch (stageNum)
			{
				case 0:
					break;
				case 1:
					activeQuests = tutorialQuests1;
					QuestUIWiter();
					break;
				case 2:
					activeQuests = tutorialQuests2;
					dialogWindowOpeners[1].CharacterDialogWindowOpen();
					break;
				case 3:
					activeQuests = tutorialQuests3;
					break;
				case 4:
					dialogWindowOpeners[2].CharacterDialogWindowOpen();
					break;
				default:
					return;
			}
			QuestUIWiter();
		}
	}


	public void QuestUIWiter()
	{
		questUIController.ClearQuests();

		foreach (Quest quest in activeQuests)
		{
			questUIController.AddQuest(quest);
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // 퀘스트 완료 시 UI를 업데이트합니다.
		}

		ResetQuestConditions(activeQuests);
	}


	IEnumerator DelayQuestClear(float delayTime, int nextDialogNumber, List<Quest> nextQuest)
	{
		yield return new WaitForSeconds(delayTime);
		activeQuests = nextQuest;
		if (nextDialogNumber >= 0)
			dialogWindowOpeners[nextDialogNumber].CharacterDialogWindowOpen();



		questUIController.ClearQuests();

		foreach (Quest quest in activeQuests)
		{
			questUIController.AddQuest(quest);
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // 퀘스트 완료 시 UI를 업데이트합니다.
		}

		ResetQuestConditions(activeQuests);
		stageNum++;
	}


	/// <summary>
	/// 각 퀘스트들의 조건들을 할당합니다. 퀘스트의 조건들이 구현되어 있습니다.
	/// </summary>
	private void QuestWriter()
	{
		Quest upQuest = new Quest();
		upQuest.questName = "W : 위로 이동";
		upQuest.isConditionMet = IsKeyUp;
		tutorialQuests1.Add(upQuest);

		Quest leftQuest = new Quest();
		leftQuest.questName = "A : 왼쪽으로 이동";
		leftQuest.isConditionMet = IsKeyLeft;
		tutorialQuests1.Add(leftQuest);

		Quest downQuest = new Quest();
		downQuest.questName = "S : 아래로 이동";
		downQuest.isConditionMet = IsKeyDown;
		tutorialQuests1.Add(downQuest);

		Quest rightQuest = new Quest();
		rightQuest.questName = "D : 오른쪽으로 이동";
		rightQuest.isConditionMet = IsKeyRight;
		tutorialQuests1.Add(rightQuest);

		Quest dashQuest = new Quest();
		dashQuest.questName = "Space : 대쉬";
		dashQuest.isConditionMet = dashQuest.isConditionMet = () => IsKeyValue("Dash");
		tutorialQuests1.Add(dashQuest);


		Quest comboAttack1 = new Quest();
		comboAttack1.questName = "J : 일반 공격";
		comboAttack1.isConditionMet = comboAttack1.isConditionMet = () => IsComboAttack(1);
		tutorialQuests2.Add(comboAttack1);

		Quest comboAttack2 = new Quest();
		comboAttack2.questName = "J-J : 콤보 공격";
		comboAttack2.isConditionMet = comboAttack2.isConditionMet = () => IsComboAttack(2);
		tutorialQuests2.Add(comboAttack2);

		Quest comboAttack3 = new Quest();
		comboAttack3.questName = "J-J-J : 콤보 공격";
		comboAttack3.isConditionMet = comboAttack3.isConditionMet = () => IsComboAttack(4);
		tutorialQuests2.Add(comboAttack3);

		Quest comboChageAttack1 = new Quest();
		comboChageAttack1.questName = "K- : 차지 공격";
		comboChageAttack1.isConditionMet = comboChageAttack1.isConditionMet = () => IsComboAttackPlus(2, PlayerInput.SpecialAttack);
		tutorialQuests3.Add(comboChageAttack1);

		Quest comboExtraAttack2 = new Quest();
		comboExtraAttack2.questName = "J-J-K : 특수 콤보 공격";
		comboExtraAttack2.isConditionMet = comboExtraAttack2.isConditionMet = () => IsComboAttackPlus(4, PlayerInput.SpecialAttack);
		tutorialQuests3.Add(comboExtraAttack2);
	
	}

		/// <summary>
		/// 퀘스트 조건을 검사합니다.
		/// </summary>
		private void CheckQuestConditions(List<Quest> quests)
	{
		foreach (Quest quest in quests)
		{
			if (quest.isConditionMet != null && quest.isConditionMet())
			{
				quest.CompleteQuest();
			}
		}
	}

	/// <summary>
	/// 모든 퀘스트 조건을 재설정합니다.
	/// </summary>
	/// <param name="quests">리셋할 퀘스트</param>
	public void ResetQuestConditions(List<Quest> quests)
	{
		foreach (Quest quest in quests)
		{
			quest.ResetQuest();
		}
	}

	/// <summary>
	/// 모든 퀘스트가 완료되었는지 확인합니다.
	/// </summary>
	/// <returns>모든 퀘스트가 완료되었으면 true를 반환하고, 그렇지 않으면 false를 반환합니다.</returns>
	private bool AllQuestsCompleted(List<Quest> quests)
	{
		foreach (Quest quest in quests)
		{
			if (!quest.IsCompleted)
			{
				return false;
			}
		}

		return true;
	}


	#region QuestEvents
	private bool IsKeyUp()
	{
		Vector3 moveInput = playerInput.actions["Move"].ReadValue<Vector3>();


		if (moveInput.y > 0) // W 키는 벡터의 y값이 양수로 매핑됩니다.
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private bool IsKeyDown()
	{
		Vector3 moveInput = playerInput.actions["Move"].ReadValue<Vector3>();


		if (moveInput.y < 0) // W 키는 벡터의 y값이 양수로 매핑됩니다.
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private bool IsKeyLeft()
	{
		Vector3 moveInput = playerInput.actions["Move"].ReadValue<Vector3>();


		if (moveInput.x < 0) // W 키는 벡터의 y값이 양수로 매핑됩니다.
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private bool IsKeyRight()
	{
		Vector3 moveInput = playerInput.actions["Move"].ReadValue<Vector3>();


		if (moveInput.x > 0) // W 키는 벡터의 y값이 양수로 매핑됩니다.
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private bool IsKeyValue(string actionKeyName)
	{
		return playerInput.actions[actionKeyName].triggered;
	}

	private bool IsComboAttack(float stNumber)
	{
		float stNum = attackNode.attackST;
		Debug.Log($"stNum : {stNum} \nstNumber : {stNumber}");
		if (stNum == stNumber)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	private bool IsComboAttackPlus(float stNumber, PlayerInput playerInput)
	{
		float stNum = attackNode.attackST;
		PlayerInput attackCommend = attackNode.command;
		Debug.Log($"stNum : {attackNode} \nstNumber : {stNumber}");
		if (stNum == stNumber && attackCommend == playerInput)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	#endregion
}

/// <summary>
/// 퀘스트를 정의하는 클래스입니다.
/// 퀘스트 이름, 필요한 키 입력, 완료 이벤트, 완료 상태를 가집니다.
/// </summary>
[System.Serializable]
public class Quest
{
	public string questName = ""; // 퀘스트 이름입니다.
	public Sprite questImage; // 퀘스트 이미지입니다. 이것은 선택 사항이며 null일 수 있습니다.
	public Sprite questClearImage; // 퀘스트 클리어 이미지입니다. 이것은 선택 사항이며 null일 수 있습니다.
	public Vector3 questPos; // 퀘스트 이미지 위치입니다. 이것은 선택 사항이며 null일 수 있습니다.
	public Func<bool> isConditionMet; // 퀘스트를 완료하기 위해 필요한 조건입니다.
	public UnityEvent<Quest> OnQuestComplete = new UnityEvent<Quest>(); // 퀘스트가 완료될 때 호출되는 이벤트입니다.
	public bool IsCompleted { get; private set; } // 퀘스트의 완료 상태입니다.


	/// <summary>
	/// 퀘스트를 완료합니다.
	/// 해당 함수를 호출하면 완료 상태가 true가 되고, 완료 이벤트가 발생합니다.
	/// </summary>
	public void CompleteQuest()
	{
		IsCompleted = true;
		OnQuestComplete.Invoke(this);
	}

	/// <summary>
	/// 퀘스트를 재설정합니다.
	/// 해당 함수를 호출하면 완료 상태가 false로 변경됩니다.
	/// </summary>
	public void ResetQuest()
	{
		IsCompleted = false;
	}
}