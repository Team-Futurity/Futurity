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

	private Dictionary<int, List<UIPerformBoard>> performBoardDic = new Dictionary<int, List<UIPerformBoard>>();
	
	private UIPerformBoard currentBoard;
	private int currentType = -1;
	
	public void AddPerformBoard(int type, UIPerformBoard board)
	{
		performBoardDic[type]?.Add(board);
	}

	public void SetPerformBoard(int type, List<UIPerformBoard> boards)
	{
		performBoardDic[type] = boards;
	}

	public void OpenPerform(int type)
	{
		currentBoard = Pop(type);
		currentBoard.Active(true);
	}

	private void GetTargetInputData(PlayerInputEnum type)
	{
		if (currentBoard == null || currentType == -1)
		{
			return;
		}
		
		var isClear = currentBoard.EnterPlayerEventType(type);

		if (isClear)
		{
			ChangeBoard();	
		}
	}

	private void ChangeBoard()
	{
		currentBoard.Active(false);
		
		currentBoard = Pop(currentType);

		if (currentBoard == null)
		{
			Debug.Log("��� �۾��� ����Ǿ����ϴ�.");
			return;
		}
		
		currentBoard.Active(true);
	}

	private UIPerformBoard Pop(int type)
	{
		if (performBoardDic[type].Count <= 0)
		{
			return null;
		}
		
		var data = performBoardDic[type][0];
		performBoardDic[type].RemoveAt(0);
		
		return data;
	}
}
