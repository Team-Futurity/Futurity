using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoardHandler : MonoBehaviour
{
	// Data�� ���޹޴� Ÿ�� ������Ʈ
	[SerializeField, Header("�÷��̾� ������")]
	private PlayerInputManager target;

	[SerializeField]
	private List<UIPerformBoard> performBoards;

	private UIPerformBoard currentBoard;

	private void Awake()
	{
		currentBoard = Pop();
		currentBoard.Active(true);
	}

	public void AddPerformBoard(UIPerformBoard board)
	{
		performBoards.Add(board);
	}

	public void SetPerformBoard(List<UIPerformBoard> boards)
	{
		performBoards = boards;
	}

	private void GetTargetInputData(PlayerInputEnum type)
	{
		var isClear = currentBoard.EnterPlayerEventType(type);

		if (isClear)
		{
			ChangeBoard();	
		}
	}

	private void ChangeBoard()
	{
		currentBoard.Active(false);
		
		currentBoard = Pop();

		if (currentBoard == null)
		{
			Debug.Log("��� �۾��� ����Ǿ����ϴ�.");
			return;
		}
		
		currentBoard.Active(true);
	}

	private UIPerformBoard Pop()
	{
		if (performBoards.Count <= 0)
		{
			return null;
		}
		
		var data = performBoards[0];
		performBoards.RemoveAt(0);
		
		return data;
	}
}
