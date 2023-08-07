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
		// UI PerformBoard에서 LastEvent를 사용하여, Call 받았을 때, next Event로 옮겨준다.
		if(performBoards is null)
		{
			FDebug.LogError($"{performBoards.GetType()}이 존재하지 않습니다.");
			Debug.Break();
		}

		// target에서 Action Event를 가져와야한다.

		// Current Board 설정
		currentIndex = 0;
		maxIndex = performBoards.Count;

		currentBoard = performBoards[currentIndex];
	}

	// Target에 들어가는 Event
	public void UpdateAction(PlayerState state)
	{
		currentBoard?.CheckedAction(state);
	}

	
	// UIPerfomBoard에 들어가는 Event Addlistnener
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
