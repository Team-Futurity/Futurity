using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPerformBoardHandler : MonoBehaviour
{
	// Data를 전달받는 타겟 오브젝트
	[SerializeField, Header("플레이어 데이터")]
	private PlayerInputManager target;

	private Dictionary<int, List<UIPerformBoard>> performBoardDic = new Dictionary<int, List<UIPerformBoard>>();
	
	private UIPerformBoard currentBoard;
	private int currentType = -1;

	[HideInInspector]
	public UnityEvent onEnded; 

	public void CreateGroup(int type)
	{
		performBoardDic.Add(type, new List<UIPerformBoard>());
	}

	public bool HasGroup(int type)
	{
		return performBoardDic.ContainsKey(type);
	}
	
	public void AddPerformBoard(int type, UIPerformBoard board)
	{
		performBoardDic[type].Add(board);
	}

	public void SetPerformBoard(int type, List<UIPerformBoard> boards)
	{
		performBoardDic[type] = boards;
	}

	public void OpenPerform(int type)
	{
		currentBoard = Pop(type);

		if (currentBoard == null)
		{
			return;
		}

		currentType = type;
		currentBoard.Active(true);
	}

	public void Debugs(PlayerInputEnum type)
	{
		GetTargetInputData(type);
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
			onEnded?.Invoke();
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
