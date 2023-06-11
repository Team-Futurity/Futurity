using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// Ʃ�丮�� �ó������� �����ϰ� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private int stageNum = 1; // ���� ���� ���� Ʃ�丮�� �ܰ踦 ��Ÿ���� �����Դϴ�.

	[SerializeField]
	private QuestUIController questUIController; // ����Ʈ UI�� �����ϴ� ��Ʈ�ѷ��Դϴ�.

	[SerializeField]
	private UnityEngine.InputSystem.PlayerInput playerInput;
	[SerializeField]
	private PlayerController playerController;
	private AttackNode attackNode;

	[SerializeField]
	private List<CharacterDialogWindowOpener> dialogWindowOpeners; // dialogWindowOpener ����Ʈ�Դϴ�.

	[SerializeField]
	private StageEndPotalController warpPotalController;

	[SerializeField]
	private UnityEvent tutorialEndEvent; // Ʃ�丮�� ������ �ܰ踦 ���� ���� ����ϴ� �̺�Ʈ�Դϴ�.

	[SerializeField]
	private List<Quest> activeQuests; // ���� Ȱ��ȭ�� ����Ʈ ����Ʈ�Դϴ�.

	[SerializeField]
	private Vector3 questImagePos; // quest �̹��� ��ġ�Դϴ�.
	[SerializeField]
	private List<Quest> tutorialQuests1; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Sprite> tutorialQuestSprites1; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Sprite> tutorialQuestClearSprites1; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Quest> tutorialQuests2; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Sprite> tutorialQuestSprites2; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Sprite> tutorialQuestClearSprites2; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Quest> tutorialQuests3; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Sprite> tutorialQuestSprites3; // tutorialQuest ����Ʈ�Դϴ�.
	[SerializeField]
	private List<Sprite> tutorialQuestClearSprites3; // tutorialQuest ����Ʈ�Դϴ�.


	/// <summary>
	/// ��ũ��Ʈ�� ���۵� �� ȣ��Ǵ� �޼����Դϴ�.
	/// ���⼭�� ��ȭâ�� ����, ��� ����Ʈ�� UI ��Ʈ�ѷ��� �߰��ϸ�, ����Ʈ �Ϸ� �� UI ������Ʈ �̺�Ʈ�� ����մϴ�.
	/// </summary>
	private void Start()
	{
		QuestWriter();

		stageNum = 0;
		activeQuests = tutorialQuests1; // ó������ ù ��° ����Ʈ ����Ʈ�� Ȱ��ȭ�մϴ�.

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
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // ����Ʈ �Ϸ� �� UI�� ������Ʈ�մϴ�.
		}
	}


	/// <summary>
	/// ����Ʈ�� ������ üũ�ϰ�, ��� ����Ʈ�� �Ϸ�Ǿ����� Ȯ���մϴ�.
	/// ����Ʈ�� �Ϸ�Ǿ��ٸ�, Ʃ�丮�� �ܰ踦 ������Ű�� ����Ʈ ������ �缳���մϴ�.
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
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // ����Ʈ �Ϸ� �� UI�� ������Ʈ�մϴ�.
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
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // ����Ʈ �Ϸ� �� UI�� ������Ʈ�մϴ�.
		}

		ResetQuestConditions(activeQuests);
		stageNum++;
	}


	/// <summary>
	/// �� ����Ʈ���� ���ǵ��� �Ҵ��մϴ�. ����Ʈ�� ���ǵ��� �����Ǿ� �ֽ��ϴ�.
	/// </summary>
	private void QuestWriter()
	{
		Quest upQuest = new Quest();
		upQuest.questName = "W : ���� �̵�";
		upQuest.isConditionMet = IsKeyUp;
		tutorialQuests1.Add(upQuest);

		Quest leftQuest = new Quest();
		leftQuest.questName = "A : �������� �̵�";
		leftQuest.isConditionMet = IsKeyLeft;
		tutorialQuests1.Add(leftQuest);

		Quest downQuest = new Quest();
		downQuest.questName = "S : �Ʒ��� �̵�";
		downQuest.isConditionMet = IsKeyDown;
		tutorialQuests1.Add(downQuest);

		Quest rightQuest = new Quest();
		rightQuest.questName = "D : ���������� �̵�";
		rightQuest.isConditionMet = IsKeyRight;
		tutorialQuests1.Add(rightQuest);

		Quest dashQuest = new Quest();
		dashQuest.questName = "Space : �뽬";
		dashQuest.isConditionMet = dashQuest.isConditionMet = () => IsKeyValue("Dash");
		tutorialQuests1.Add(dashQuest);


		Quest comboAttack1 = new Quest();
		comboAttack1.questName = "J : �Ϲ� ����";
		comboAttack1.isConditionMet = comboAttack1.isConditionMet = () => IsComboAttack(1);
		tutorialQuests2.Add(comboAttack1);

		Quest comboAttack2 = new Quest();
		comboAttack2.questName = "J-J : �޺� ����";
		comboAttack2.isConditionMet = comboAttack2.isConditionMet = () => IsComboAttack(2);
		tutorialQuests2.Add(comboAttack2);

		Quest comboAttack3 = new Quest();
		comboAttack3.questName = "J-J-J : �޺� ����";
		comboAttack3.isConditionMet = comboAttack3.isConditionMet = () => IsComboAttack(4);
		tutorialQuests2.Add(comboAttack3);

		Quest comboChageAttack1 = new Quest();
		comboChageAttack1.questName = "K- : ���� ����";
		comboChageAttack1.isConditionMet = comboChageAttack1.isConditionMet = () => IsComboAttackPlus(2, PlayerInput.SpecialAttack);
		tutorialQuests3.Add(comboChageAttack1);

		Quest comboExtraAttack2 = new Quest();
		comboExtraAttack2.questName = "J-J-K : Ư�� �޺� ����";
		comboExtraAttack2.isConditionMet = comboExtraAttack2.isConditionMet = () => IsComboAttackPlus(4, PlayerInput.SpecialAttack);
		tutorialQuests3.Add(comboExtraAttack2);
	
	}

		/// <summary>
		/// ����Ʈ ������ �˻��մϴ�.
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
	/// ��� ����Ʈ ������ �缳���մϴ�.
	/// </summary>
	/// <param name="quests">������ ����Ʈ</param>
	public void ResetQuestConditions(List<Quest> quests)
	{
		foreach (Quest quest in quests)
		{
			quest.ResetQuest();
		}
	}

	/// <summary>
	/// ��� ����Ʈ�� �Ϸ�Ǿ����� Ȯ���մϴ�.
	/// </summary>
	/// <returns>��� ����Ʈ�� �Ϸ�Ǿ����� true�� ��ȯ�ϰ�, �׷��� ������ false�� ��ȯ�մϴ�.</returns>
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


		if (moveInput.y > 0) // W Ű�� ������ y���� ����� ���ε˴ϴ�.
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


		if (moveInput.y < 0) // W Ű�� ������ y���� ����� ���ε˴ϴ�.
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


		if (moveInput.x < 0) // W Ű�� ������ y���� ����� ���ε˴ϴ�.
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


		if (moveInput.x > 0) // W Ű�� ������ y���� ����� ���ε˴ϴ�.
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
/// ����Ʈ�� �����ϴ� Ŭ�����Դϴ�.
/// ����Ʈ �̸�, �ʿ��� Ű �Է�, �Ϸ� �̺�Ʈ, �Ϸ� ���¸� �����ϴ�.
/// </summary>
[System.Serializable]
public class Quest
{
	public string questName = ""; // ����Ʈ �̸��Դϴ�.
	public Sprite questImage; // ����Ʈ �̹����Դϴ�. �̰��� ���� �����̸� null�� �� �ֽ��ϴ�.
	public Sprite questClearImage; // ����Ʈ Ŭ���� �̹����Դϴ�. �̰��� ���� �����̸� null�� �� �ֽ��ϴ�.
	public Vector3 questPos; // ����Ʈ �̹��� ��ġ�Դϴ�. �̰��� ���� �����̸� null�� �� �ֽ��ϴ�.
	public Func<bool> isConditionMet; // ����Ʈ�� �Ϸ��ϱ� ���� �ʿ��� �����Դϴ�.
	public UnityEvent<Quest> OnQuestComplete = new UnityEvent<Quest>(); // ����Ʈ�� �Ϸ�� �� ȣ��Ǵ� �̺�Ʈ�Դϴ�.
	public bool IsCompleted { get; private set; } // ����Ʈ�� �Ϸ� �����Դϴ�.


	/// <summary>
	/// ����Ʈ�� �Ϸ��մϴ�.
	/// �ش� �Լ��� ȣ���ϸ� �Ϸ� ���°� true�� �ǰ�, �Ϸ� �̺�Ʈ�� �߻��մϴ�.
	/// </summary>
	public void CompleteQuest()
	{
		IsCompleted = true;
		OnQuestComplete.Invoke(this);
	}

	/// <summary>
	/// ����Ʈ�� �缳���մϴ�.
	/// �ش� �Լ��� ȣ���ϸ� �Ϸ� ���°� false�� ����˴ϴ�.
	/// </summary>
	public void ResetQuest()
	{
		IsCompleted = false;
	}
}