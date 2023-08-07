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
			FDebug.LogError($"{performBoards.GetType()}�� �������� �ʽ��ϴ�.");
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

	// Target�� ���� Event
	public void UpdateAction(PlayerState state)
	{
		currentBoard?.CheckedAction(state);
	}

	
	// UIPerfomBoard�� ���� Event Addlistnener
	public void CheckAllPass()
	{
		// �� �ð� ���� ���𰡰� ������ ��.
		
		OnNextBoard();
	}

	private void OnNextBoard()
	{
		if (currentIndex + 1 >= maxIndex)
		{
			// Next Board�� Ended�� ��.
			
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
