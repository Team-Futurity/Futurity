using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPerformActionHandler : MonoBehaviour
{
	[field: SerializeField] public PlayerController Target { get; private set; }

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
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Z))
			OnNextBoard();
		
		if(Input.GetKeyDown(KeyCode.X))
			OnBeforeBoard();
	}

	// Target에 들어가는 Event
	public void UpdateAction(PlayerState state)
	{
		currentBoard?.CheckedAction(state);
	}

	
	// UIPerfomBoard에 들어가는 Event Addlistnener
	public void CheckAllPass()
	{
		// 이 시간 동안 무언가가 켜져야 함.
		
		OnNextBoard();
	}

	private void OnNextBoard()
	{
		if (currentIndex + 1 >= maxIndex)
		{
			// Next Board가 Ended일 때.
			
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
		currentBoard.lastClearEvent.RemoveAllListeners();
		currentBoard.gameObject.SetActive(false);

		currentBoard = performBoards[currentIndex];
		
		currentBoard.lastClearEvent.AddListener(CheckAllPass);
		currentBoard.gameObject.SetActive(true);
	}
}
