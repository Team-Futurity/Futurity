using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPerformActionHandler : MonoBehaviour
{
	[field: SerializeField] public PlayerController target { get; private set; }

	[SerializeField]
	private List<UIPerformBoard> performBoards;

	[SerializeField]
	private UIPerformBoard currentBoard;

	[SerializeField]
	private UIPerformBoard nextBoard;

	private int currentIndex = 0;
	private int maxIndex = 0;

	private void Awake()
	{
		// UI PerformBoard���� LastEvent�� ����Ͽ�, Call �޾��� ��, next Event�� �Ű��ش�.
		if(performBoards is null)
		{
			FDebug.LogError($"{performBoards.GetType()}�� �������� �ʽ��ϴ�.");
			Debug.Break();
		}

		// target���� Action Event�� �����;��Ѵ�.

		// Current Board ����
		currentIndex = 0;
		maxIndex = performBoards.Count;

		currentBoard = performBoards[currentIndex];
	}

	// Target�� ���� Event
	public void UpdateAction(PlayerState state)
	{
		currentBoard?.CheckedAction(state);
	}

	
	// UIPerfomBoard�� ���� Event Addlistnener
	public void CheckAllPass()
	{
		OnChangeBoard();
	}

	private void OnChangeBoard()
	{
		if(currentBoard != null)
		{
			currentBoard.lastClearEvent = null;
		}

		currentBoard = nextBoard;
		currentBoard.lastClearEvent.AddListener(CheckAllPass);

		SetNextBoard();

		currentIndex++;
	}

	private void SetNextBoard()
	{

	}
}
