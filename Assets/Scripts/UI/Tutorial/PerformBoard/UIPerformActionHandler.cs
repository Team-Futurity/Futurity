using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPerformActionHandler : MonoBehaviour
{
	[field: SerializeField] public PlayerInputManager Target { get; private set; }

	[SerializeField]
	private List<UIPerformBoard> performBoards;

	private UIPerformBoard currentBoard;

	private int currentIndex = 0;
	private int maxIndex = 0;

	private void Awake()
	{
		if(performBoards is null)
		{
			FDebug.LogError($"{performBoards.GetType()}이 존재하지 않습니다.");
			Debug.Break();
		}

		currentIndex = 0;
		maxIndex = performBoards.Count;

		currentBoard = performBoards[currentIndex];
		
		Target.onChangeStateEvent.AddListener(UpdateAction);
		currentBoard.onLastClearEvent.AddListener(CheckAllPass);
	}

	// Target에 들어가는 Event
	public void UpdateAction(PlayerInputEnum data)
	{
		currentBoard?.CheckedAction(data);
	}

	
	// UIPerfomBoard에 들어가는 Event Addlistnener
	public void CheckAllPass()
	{
		OnNextBoard();
	}

	private void OnNextBoard()
	{
		if (currentIndex + 1 >= maxIndex)
		{
			// Next Board가 Ended일 때.
			FDebug.Log("Tutorial Clear ! ! !");
			return;
		}
		
		currentIndex++;

		SetBoard();
	}

	private void OnBeforeBoard()
	{
		if (currentIndex - 1 < 0)
		{
			return;
		}
		
		currentIndex--;
		
		SetBoard();
	}

	private void SetBoard()
	{
		currentBoard.onLastClearEvent.RemoveAllListeners();
		currentBoard.gameObject.SetActive(false);

		currentBoard = performBoards[currentIndex];
		
		currentBoard.onLastClearEvent.AddListener(CheckAllPass);
		currentBoard.gameObject.SetActive(true);
	}
}
