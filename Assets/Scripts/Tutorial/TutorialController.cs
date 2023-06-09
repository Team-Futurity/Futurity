using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Ʃ�丮�� �ó������� �����ϰ� �����ϴ� Ŭ�����Դϴ�.
/// </summary>
public class TutorialController : MonoBehaviour
{
	[SerializeField]
	private int stageNum; // ���� ���� ���� Ʃ�丮�� �ܰ踦 ��Ÿ���� �����Դϴ�.

	[SerializeField]
	private QuestUIController questUIController; // ����Ʈ UI�� �����ϴ� ��Ʈ�ѷ��Դϴ�.

	[SerializeField]
	private List<CharacterDialogWindowOpener> dialogWindowOpeners; // dialogWindowOpener ����Ʈ�Դϴ�.

	[SerializeField]
	private List<Quest> tutorialQuests; // tutorialQuest ����Ʈ�Դϴ�.

	/// <summary>
	/// ��ũ��Ʈ�� ���۵� �� ȣ��Ǵ� �޼����Դϴ�.
	/// ���⼭�� ��ȭâ�� ����, ��� ����Ʈ�� UI ��Ʈ�ѷ��� �߰��ϸ�, ����Ʈ �Ϸ� �� UI ������Ʈ �̺�Ʈ�� ����մϴ�.
	/// </summary>
	private void Start()
	{
		dialogWindowOpeners[1].CharacterDialogWindowOpen();

		foreach (Quest quest in tutorialQuests)
		{
			questUIController.AddQuest(quest);
			quest.OnQuestComplete.AddListener(questUIController.UpdateQuestUI); // ����Ʈ �Ϸ� �� UI�� ������Ʈ�մϴ�.
		}
	}

	/// <summary>
	/// �� �����Ӹ��� ȣ��Ǵ� �޼����Դϴ�.
	/// ���⼭�� ����Ʈ�� ������ üũ�ϰ�, ��� ����Ʈ�� �Ϸ�Ǿ����� Ȯ���մϴ�.
	/// ��� ����Ʈ�� �Ϸ�Ǿ��ٸ�, Ʃ�丮�� �ܰ踦 ������Ű�� ����Ʈ ������ �缳���մϴ�.
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
	/// ����Ʈ ������ Ȯ���ϰ� �ش� Ű�� ���ȴ��� Ȯ���մϴ�.
	/// ������ �����Ǹ� �ش� ����Ʈ�� �Ϸ��մϴ�.
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
	/// ��� ����Ʈ�� ������ �缳���մϴ�.
	/// </summary>
	public void ResetQuestConditions()
	{
		foreach (Quest quest in tutorialQuests)
		{
			quest.ResetQuest();
		}
	}

	/// <summary>
	/// ��� ����Ʈ�� �Ϸ�Ǿ����� Ȯ���մϴ�.
	/// </summary>
	/// <returns>��� ����Ʈ�� �Ϸ�Ǿ��ٸ� true�� ��ȯ�ϰ�, �׷��� �ʴٸ� false�� ��ȯ�մϴ�.</returns>
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
/// ����Ʈ�� �����ϴ� Ŭ�����Դϴ�.
/// ����Ʈ �̸�, �ʿ��� Ű �Է�, �Ϸ� �̺�Ʈ, �Ϸ� ���¸� �����ϴ�.
/// </summary>
[System.Serializable]
public class Quest
{
	public string questName; // ����Ʈ �̸��Դϴ�.
	public string requiredKey; // ����Ʈ�� �Ϸ��ϱ� ���� �ʿ��� Ű �ڵ��Դϴ�.
	public UnityEvent<Quest> OnQuestComplete = new UnityEvent<Quest>(); // ����Ʈ�� �Ϸ�� �� ȣ��Ǵ� �̺�Ʈ�Դϴ�.
	public bool IsCompleted { get; private set; } // ����Ʈ�� �Ϸ� �����Դϴ�.


	/// <summary>
	/// ����Ʈ�� �Ϸ��մϴ�.
	/// �� �޼��带 ȣ���ϸ� �Ϸ� ���°� true�� �ǰ�, �Ϸ� �̺�Ʈ�� �߻��մϴ�.
	/// </summary>
	public void CompleteQuest()
	{
		IsCompleted = true;
		OnQuestComplete.Invoke(this);
	}

	/// <summary>
	/// ����Ʈ�� �缳���մϴ�.
	/// �� �޼��带 ȣ���ϸ� �Ϸ� ���°� false�� ����˴ϴ�.
	/// </summary>
	public void ResetQuest()
	{
		IsCompleted = false;
	}
}